/*using Microsoft.AspNetCore.Authorization;
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

        private const string PRECIO = "24.99";
        private const string MONEDA = "USD";

        public PaymentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // =============================================
        // PASO 1: Crear la orden de PayPal
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
        // PASO 2: Capturar el pago y activar Premium
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
                    // ✅ FIX 1: Usar ClaimTypes.Email en lugar de NameIdentifier
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                    if (string.IsNullOrEmpty(userEmail))
                        return Unauthorized(new { message = "No se pudo identificar al usuario" });

                    var usuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.Email == userEmail);

                    if (usuario != null)
                    {
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
        // Método auxiliar: Obtener Access Token de PayPal
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
    }

    // Modelo para recibir el OrderId
    public class CapturarPagoRequest
    {
        public string OrderId { get; set; } = string.Empty;
    }
}*/

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
}