using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Relatosxxx.Data;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ImagenesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Imagenes/random
        [HttpGet("random")]
        public async Task<IActionResult> GetRandomImage()
        {
            var imagenes = await _context.ImagenDecorativas
                .Where(i => i.Activa)
                .ToListAsync();

            if (imagenes.Count == 0)
                return NotFound(new { url = "" });

            var random = new Random();
            var elegida = imagenes[random.Next(imagenes.Count)];

            return Ok(new { url = elegida.Url, categoria = elegida.Categoria });
        }
    }
}