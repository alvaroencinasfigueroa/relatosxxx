using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Relatosxxx.Data;
using Relatosxxx.Models;
using System.Security.Claims;

namespace Relatosxxx.Controllers
{
    [Route("api/Relatos")]
    [ApiController]
    public class RelatosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RelatosApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Relatos?page=1&pageSize=12
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 12)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 12;

            var query = _context.Relatos.OrderByDescending(r => r.FechaCreacion);

            var total = await query.CountAsync();

            var relatos = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                data = relatos,
                total = total,
                page = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)total / pageSize)
            });
        }

        // GET: api/Relatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Relato>> GetById(int id)
        {
            var relato = await _context.Relatos.FindAsync(id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });
            return Ok(relato);
        }

        // POST: api/Relatos
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Relato>> Create([FromBody] Relato nuevoRelato)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(nuevoRelato.ImagenUrl))
                nuevoRelato.ImagenUrl = "https://via.placeholder.com/400x200/ff69b4/ffffff?text=Relato";

            _context.Relatos.Add(nuevoRelato);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = nuevoRelato.Id }, nuevoRelato);
        }

        // PUT: api/Relatos/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(int id, [FromBody] Relato relatoActualizado)
        {
            var relato = await _context.Relatos.FindAsync(id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            relato.Titulo = relatoActualizado.Titulo;
            relato.Categoria = relatoActualizado.Categoria;
            relato.Contenido = relatoActualizado.Contenido;
            relato.ImagenUrl = relatoActualizado.ImagenUrl;
            relato.EsPremium = relatoActualizado.EsPremium;

            await _context.SaveChangesAsync();
            return Ok(relato);
        }

        // DELETE: api/Relatos/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var relato = await _context.Relatos.FindAsync(id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            _context.Relatos.Remove(relato);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Relato eliminado exitosamente" });
        }
    }
}