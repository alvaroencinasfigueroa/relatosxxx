using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relatosxxx.Data;
using Relatosxxx.Models;
using System.Security.Claims;
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

        // ==========================================
        // VARIABLES DE PRECIOS CRIPTO
        // ==========================================
        private const string PRECIO_TON = "25";  // 25 TON
        private const string PRECIO_USDT = "25"; // 25 USDT

        public PaymentController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                // 1. Identificar al usuario
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "No se pudo identificar al usuario" });

                int userId = int.Parse(userIdClaim);

                // 2. Verificar si este memo ya fue procesado
                var pagoExistente = await _context.Pagos
                    .FirstOrDefaultAsync(p => p.Identificador == request.Memo && p.Metodo == "TON");

                if (pagoExistente != null && pagoExistente.Procesado)
                {
                    // Ya se procesó anteriormente
                    var usuarioActual = await _context.Usuarios.FindAsync(userId);
                    return Ok(new { message = "Este pago ya fue reclamado anteriormente.", isPremium = usuarioActual?.IsPremium ?? false });
                }

                // 3. Consultar blockchain
                var tonWalletAddress = _configuration["Ton:WalletAddress"];
                var apiUrl = $"https://toncenter.com/api/v2/getTransactions?address={tonWalletAddress}&limit=30"; // Aumenta un poco el límite
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Error al consultar la blockchain de TON" });

                var responseBody = await response.Content.ReadAsStringAsync();
                bool pagoValidado = ValidarTransaccionEnJson(responseBody, request.Memo, PRECIO_TON);

                if (!pagoValidado)
                    return BadRequest(new { message = "El pago no se ha reflejado en la blockchain aún o el memo es incorrecto." });

                // 4. Pago válido: registrar en tabla Pagos (si no existía) y activar Premium
                if (pagoExistente == null)
                {
                    _context.Pagos.Add(new Pago
                    {
                        UsuarioId = userId,
                        Metodo = "TON",
                        Identificador = request.Memo,
                        Monto = decimal.Parse(PRECIO_TON),
                        Fecha = DateTime.UtcNow,
                        Procesado = true
                    });
                }
                else
                {
                    pagoExistente.Procesado = true;
                }

                var usuario = await _context.Usuarios.FindAsync(userId);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado." });

                if (usuario.IsPremium)
                    return Ok(new { message = "El usuario ya era Premium.", isPremium = true });

                usuario.IsPremium = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "¡Pago con TON exitoso! Bienvenido al club VIP 🎉", isPremium = true });
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
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userIdClaim))
                    return Unauthorized(new { message = "No se pudo identificar al usuario" });

                int userId = int.Parse(userIdClaim);
                if (string.IsNullOrWhiteSpace(request.TxId))
                    return BadRequest(new { message = "El TxID no puede estar vacío." });

                // 1. Verificar si ya existe ese TxID en nuestra BD
                var pagoExistente = await _context.Pagos
                    .FirstOrDefaultAsync(p => p.Identificador == request.TxId && p.Metodo == "USDT");

                if (pagoExistente != null && pagoExistente.Procesado)
                {
                    var usuarioActual = await _context.Usuarios.FindAsync(userId);
                    return Ok(new { message = "Este pago ya fue reclamado anteriormente.", isPremium = usuarioActual?.IsPremium ?? false });
                }

                // 2. Validar en blockchain
                var direccionEsperada = _configuration["Usdt:DireccionTrc20"]?.ToLower();
                var apiUrl = $"https://api.trongrid.io/v1/transactions/{request.TxId}/events";
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                    return BadRequest(new { message = "Error al consultar la blockchain de Tron." });

                var body = await response.Content.ReadAsStringAsync();
                bool pagoValido = ValidarTransaccionUsdtTrc20(body, direccionEsperada, PRECIO_USDT);

                if (!pagoValido)
                    return BadRequest(new { message = "No se pudo verificar el pago. Revisa el TxID e intenta de nuevo en un minuto." });

                // 3. Pago válido: registrar y activar Premium
                if (pagoExistente == null)
                {
                    _context.Pagos.Add(new Pago
                    {
                        UsuarioId = userId,
                        Metodo = "USDT",
                        Identificador = request.TxId,
                        Monto = decimal.Parse(PRECIO_USDT),
                        Fecha = DateTime.UtcNow,
                        Procesado = true
                    });
                }
                else
                {
                    pagoExistente.Procesado = true;
                }

                var usuario = await _context.Usuarios.FindAsync(userId);
                if (usuario == null)
                    return NotFound(new { message = "Usuario no encontrado." });

                if (usuario.IsPremium)
                    return Ok(new { message = "El usuario ya era Premium.", isPremium = true });

                usuario.IsPremium = true;
                await _context.SaveChangesAsync();

                return Ok(new { message = "¡Pago con USDT exitoso! Bienvenido al club VIP 🎉", isPremium = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno", detalle = ex.Message });
            }
        }
        // =============================================
        // MÉTODOS AUXILIARES Y DE VALIDACIÓN
        // =============================================
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
                // Si falla el parseo, el pago no se procesa
            }
            return false;
        }

        private bool ValidarTransaccionUsdtTrc20(string jsonResponse, string? direccionEsperada, string montoUsdt)
        {
            try
            {
                // USDT TRC20 tiene 6 decimales (25 USDT = 25,000,000 en la blockchain)
                long montoEsperado = long.Parse(montoUsdt) * 1_000_000;

                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var data = doc.RootElement.GetProperty("data");

                foreach (var evento in data.EnumerateArray())
                {
                    if (!evento.TryGetProperty("event_name", out var eventName)) continue;
                    if (eventName.GetString() != "Transfer") continue;

                    var result = evento.GetProperty("result");

                    if (!result.TryGetProperty("to", out var toElement)) continue;
                    string? to = toElement.GetString()?.ToLower();
                    if (to != direccionEsperada) continue;

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
    // DTOs PARA LAS PETICIONES (SOLO CRIPTO)
    // =============================================
    public class VerificarPagoTonRequest
    {
        public string Memo { get; set; } = string.Empty;
    }

    public class VerificarPagoUsdtRequest
    {
        public string TxId { get; set; } = string.Empty;
    }
}