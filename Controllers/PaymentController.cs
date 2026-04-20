using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relatosxxx.Data;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new HttpClient();

        // Variables PayPal
        private const string PRECIO = "24.99";
        private const string MONEDA = "USD";

        // Variables TON
        private const string PRECIO_TON = "25"; // Precio fijo en TON

        // Variables Bybit
        private const string PRECIO_USDT = "25";

        public PaymentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // =============================================
        // PASO 1 (PAYPAL): Crear la orden de PayPal
        // POST: api/Payment/crear-orden
        // =============================================
        [HttpPost("crear-orden")]
        [Authorize]
        public async Task<IActionResult> CrearOrden()
        {
            try
            {
                var accessToken = await ObtenerAccessToken();

                var orden = new
                {
                    intent = "CAPTURE",
                    purchase_units = new[]
                    {
                        new
                        {
                            amount = new
                            {
                                currency_code = MONEDA,
                                value = PRECIO
                            },
                            description = "Acceso Premium de por vida - RelatosXXX"
                        }
                    }
                };

                var baseUrl = _configuration["PayPal:BaseUrl"] ?? "https://api-m.sandbox.paypal.com";
                var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/v2/checkout/orders");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Content = new StringContent(JsonSerializer.Serialize(orden), Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Error al crear la orden", detalle = responseBody });

                var ordenPayPal = JsonSerializer.Deserialize<JsonElement>(responseBody);
                var orderId = ordenPayPal.GetProperty("id").GetString();

                return Ok(new { orderId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detalle = ex.Message });
            }
        }

        // =============================================
        // PASO 2 (PAYPAL): Capturar el pago y activar Premium
        // POST: api/Payment/capturar-pago
        // =============================================
        [HttpPost("capturar-pago")]
        [Authorize]
        public async Task<IActionResult> CapturarPago([FromBody] CapturarPagoRequest request)
        {
            try
            {
                var accessToken = await ObtenerAccessToken();

                var baseUrl = _configuration["PayPal:BaseUrl"] ?? "https://api-m.sandbox.paypal.com";
                var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                    $"{baseUrl}/v2/checkout/orders/{request.OrderId}/capture");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                httpRequest.Content = new StringContent("{}", Encoding.UTF8, "application/json");

                var response = await _httpClient.SendAsync(httpRequest);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Error al capturar el pago", detalle = responseBody });

                var captureData = JsonSerializer.Deserialize<JsonElement>(responseBody);
                var status = captureData.GetProperty("status").GetString();

                if (status == "COMPLETED")
                {
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                    if (string.IsNullOrEmpty(userEmail))
                        return Unauthorized(new { message = "No se pudo identificar al usuario" });

                    var usuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (usuario != null)
                    {
                        if (usuario.IsPremium) return Ok(new { message = "El usuario ya era Premium.", isPremium = true });

                        usuario.IsPremium = true;
                        await _context.SaveChangesAsync();

                        return Ok(new
                        {
                            message = "¡Pago exitoso! Bienvenido al club Premium 🎉",
                            isPremium = true
                        });
                    }

                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return BadRequest(new { message = "El pago no se completó", status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detalle = ex.Message });
            }
        }

        // =============================================
        // PASO 1 (TON): Generar los datos para el pago
        // GET: api/Payment/ton/generar-pago
        // =============================================
        [HttpGet("ton/generar-pago")]
        [Authorize]
        public IActionResult GenerarPagoTon()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "No se pudo identificar al usuario" });

            // Leemos la wallet de appsettings.json
            var tonWalletAddress = _configuration["Ton:WalletAddress"];
            if (string.IsNullOrEmpty(tonWalletAddress))
                return StatusCode(500, new { message = "La wallet de TON no está configurada en el servidor." });

            // Generamos el Memo único
            string memo = $"Premium-{Guid.NewGuid().ToString("N").Substring(0, 8)}";

            return Ok(new
            {
                address = tonWalletAddress,
                amount = PRECIO_TON,
                memo = memo
            });
        }

        // =============================================
        // PASO 2 (TON): Verificar en la blockchain y activar
        // POST: api/Payment/ton/verificar-pago
        // =============================================
        [HttpPost("ton/verificar-pago")]
        [Authorize]
        public async Task<IActionResult> VerificarPagoTon([FromBody] VerificarPagoTonRequest request)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized(new { message = "No se pudo identificar al usuario" });

                var tonWalletAddress = _configuration["Ton:WalletAddress"];

                // Consultamos a Toncenter para ver las últimas transacciones de tu wallet
                var apiUrl = $"https://toncenter.com/api/v2/getTransactions?address={tonWalletAddress}&limit=20";

                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Error al consultar la blockchain de TON" });

                var responseBody = await response.Content.ReadAsStringAsync();

                // Verificamos si en esas transacciones existe una con el Memo y monto correcto
                bool pagoValidado = ValidarTransaccionEnJson(responseBody, request.Memo, PRECIO_TON);

                if (pagoValidado)
                {
                    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == userEmail);
                    if (usuario != null)
                    {
                        if (usuario.IsPremium) return Ok(new { message = "El usuario ya era Premium.", isPremium = true });

                        usuario.IsPremium = true;
                        await _context.SaveChangesAsync();

                        return Ok(new { message = "¡Pago con TON exitoso! Bienvenido al club Premium 🎉", isPremium = true });
                    }
                    return NotFound(new { message = "Usuario no encontrado en la base de datos" });
                }

                return BadRequest(new { message = "El pago no se ha reflejado en la blockchain aún o el memo es incorrecto." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detalle = ex.Message });
            }
        }

        // =============================================
        // USDT TRC20: Obtener dirección de depósito
        // GET: api/Payment/usdt/obtener-direccion
        // =============================================
        [HttpGet("usdt/obtener-direccion")]
        [Authorize]
        public IActionResult ObtenerDireccionUsdt()
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized(new { message = "No se pudo identificar al usuario" });

            var direccion = _configuration["Usdt:DireccionTrc20"];
            if (string.IsNullOrEmpty(direccion))
                return StatusCode(500, new { message = "Dirección USDT no configurada en el servidor." });

            return Ok(new
            {
                direccion = direccion,
                monto = PRECIO_USDT,
                red = "TRC20 (Tron)"
            });
        }

        // =============================================
        // USDT TRC20: Verificar pago por TxID
        // POST: api/Payment/usdt/verificar-pago
        // =============================================
        [HttpPost("usdt/verificar-pago")]
        [Authorize]
        public async Task<IActionResult> VerificarPagoUsdt([FromBody] VerificarPagoUsdtRequest request)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized(new { message = "No se pudo identificar al usuario" });

                if (string.IsNullOrWhiteSpace(request.TxId))
                    return BadRequest(new { message = "El TxID no puede estar vacío." });

                var direccionEsperada = _configuration["Usdt:DireccionTrc20"]?.ToLower();

                // Consultar la blockchain de Tron
                var apiUrl = $"https://api.trongrid.io/v1/transactions/{request.TxId}/events";
                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Error al consultar la blockchain de Tron." });

                var body = await response.Content.ReadAsStringAsync();
                bool pagoValido = ValidarTransaccionUsdtTrc20(body, direccionEsperada, PRECIO_USDT);

                if (pagoValido)
                {
                    var usuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (usuario == null)
                        return NotFound(new { message = "Usuario no encontrado." });

                    if (usuario.IsPremium)
                        return Ok(new { message = "El usuario ya era Premium.", isPremium = true });

                    usuario.IsPremium = true;
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "¡Pago con USDT exitoso! Bienvenido al club Premium 🎉", isPremium = true });
                }

                return BadRequest(new { message = "No se pudo verificar el pago. Revisa el TxID e intenta de nuevo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detalle = ex.Message });
            }
        }


        // =============================================
        // Métodos auxiliares
        // =============================================
        private async Task<string> ObtenerAccessToken()
        {
            var clientId = _configuration["PayPal:ClientId"];
            var clientSecret = _configuration["PayPal:ClientSecret"];
            var baseUrl = _configuration["PayPal:BaseUrl"] ?? "https://api-m.sandbox.paypal.com";

            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var tokenData = JsonSerializer.Deserialize<JsonElement>(body);
            return tokenData.GetProperty("access_token").GetString() ?? "";
        }

        private bool ValidarTransaccionEnJson(string jsonResponse, string memoEsperado, string montoEsperadoTon)
        {
            try
            {
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var result = doc.RootElement.GetProperty("result");

                // 1 TON = 1,000,000,000 NanoTons
                long montoEsperadoNano = long.Parse(montoEsperadoTon) * 1000000000;

                foreach (var tx in result.EnumerateArray())
                {
                    var inMsg = tx.GetProperty("in_msg");

                    if (inMsg.TryGetProperty("value", out JsonElement valueElement))
                    {
                        long valorRecibido = long.Parse(valueElement.GetString() ?? "0");

                        if (inMsg.TryGetProperty("message", out JsonElement messageElement))
                        {
                            string memoRecibido = messageElement.GetString() ?? "";

                            // Validar Memo y que el monto sea el acordado (o superior por seguridad)
                            if (memoRecibido == memoEsperado && valorRecibido >= montoEsperadoNano)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                // Si falla el parseo, simplemente retornamos falso y el pago no se procesa
            }
            return false;
        }

        private bool ValidarTransaccionUsdtTrc20(string jsonResponse, string? direccionEsperada, string montoUsdt)
        {
            try
            {
                // USDT TRC20 tiene 6 decimales
                // 25 USDT = 25,000,000 en unidades mínimas
                long montoEsperado = long.Parse(montoUsdt) * 1_000_000;

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var data = doc.RootElement.GetProperty("data");

                foreach (var evento in data.EnumerateArray())
                {
                    // Verificar que sea un evento de transferencia USDT
                    if (!evento.TryGetProperty("event_name", out var eventName)) continue;
                    if (eventName.GetString() != "Transfer") continue;

                    var result = evento.GetProperty("result");

                    // Verificar destinatario
                    if (!result.TryGetProperty("to", out var toElement)) continue;
                    string? to = toElement.GetString()?.ToLower();
                    if (to != direccionEsperada) continue;

                    // Verificar monto
                    if (!result.TryGetProperty("value", out var valueElement)) continue;
                    if (!long.TryParse(valueElement.GetString(), out long valorRecibido)) continue;

                    if (valorRecibido >= montoEsperado)
                        return true;
                }
            }
            catch { }

            return false;
        }

    }



    // =============================================
    // DTOs para las peticiones
    // =============================================
    public class CapturarPagoRequest
    {
        public string OrderId { get; set; } = string.Empty;
    }

    public class VerificarPagoTonRequest
    {
        public string Memo { get; set; } = string.Empty;
    }

    public class VerificarPagoUsdtRequest 
    {
        public string TxId { get; set; } = string.Empty;
    }

}