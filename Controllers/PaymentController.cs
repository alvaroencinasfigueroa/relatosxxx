using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relatosxxx.Data;
using System.Net.Http.Headers;
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

        // Precio del acceso premium de por vida
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
        [Authorize] // El usuario debe estar logueado
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
        [Authorize] // El usuario debe estar logueado
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
                    // ✅ PAGO EXITOSO: Activar Premium en la base de datos
                    var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("sub")?.Value;

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
}