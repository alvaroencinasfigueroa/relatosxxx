using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relatosxxx.Data;

namespace Relatosxxx.Controllers
{
    [Route("relato")]
    public class ShareController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ShareController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: /relato/{id}
        // Esta ruta la usan los bots de Twitter, WhatsApp, Facebook para leer los meta tags
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Share(int id)
        {
            var relato = await _context.Relatos.FindAsync(id);

            var baseUrl = _configuration["AppUrl"] ?? "https://relatosxxx.onrender.com";

            if (relato == null)
            {
                // Relato no encontrado: redirige al inicio
                return Redirect(baseUrl);
            }

            // Preview del contenido (primeros 150 caracteres)
            var descripcion = relato.Contenido?.Length > 150
                ? relato.Contenido.Substring(0, 150) + "..."
                : relato.Contenido ?? "";

            // Imagen: usa la del relato o una por defecto
            var imagen = !string.IsNullOrEmpty(relato.ImagenUrl)
                ? relato.ImagenUrl
                : $"{baseUrl}/img/og-default.jpg";

            var titulo = System.Net.WebUtility.HtmlEncode(relato.Titulo);
            var desc = System.Net.WebUtility.HtmlEncode(descripcion);
            var urlRelato = $"{baseUrl}/relato/{id}";

            // HTML mínimo con meta tags Open Graph + redirección al SPA
            var html = $@"<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <title>{titulo} 🔥 Relatos Sensuales</title>

    <!-- Open Graph (Facebook, WhatsApp, Telegram) -->
    <meta property=""og:type"" content=""article"">
    <meta property=""og:url"" content=""{urlRelato}"">
    <meta property=""og:title"" content=""{titulo} 🔥"">
    <meta property=""og:description"" content=""{desc}"">
    <meta property=""og:image"" content=""{imagen}"">
    <meta property=""og:site_name"" content=""Relatos Sensuales"">
    <meta property=""og:locale"" content=""es_ES"">

    <!-- Twitter Card -->
    <meta name=""twitter:card"" content=""summary_large_image"">
    <meta name=""twitter:title"" content=""{titulo} 🔥"">
    <meta name=""twitter:description"" content=""{desc}"">
    <meta name=""twitter:image"" content=""{imagen}"">

    <!-- Redirige al usuario real al SPA -->
    <meta http-equiv=""refresh"" content=""0;url=/#relato-{id}"">
</head>
<body>
    <p>Redirigiendo...</p>
    <script>window.location.href = '/#relato-{id}';</script>
</body>
</html>";

            return Content(html, "text/html");
        }
    }
}