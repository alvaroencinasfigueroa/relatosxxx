using Microsoft.AspNetCore.Mvc;
using Relatosxxx.Models;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatosController : ControllerBase
    {
        // Lista temporal en memoria (después usaremos BD)
        private static List<Relato> _relatos = new List<Relato>
        {
            new Relato
            {
                Id = 1,
                Titulo = "Susurros en la Penumbra",
                Contenido = "La habitación estaba sumida en sombras seductoras, apenas iluminada por la luz de la luna que se filtraba entre las cortinas de seda...",
                EsPremium = false,
                FechaCreacion = DateTime.Now
            },
            new Relato
            {
                Id = 2,
                Titulo = "Terciopelo y Pecado",
                Contenido = "El salón privado olía a perfume francés y deseo contenido. Las paredes forradas en terciopelo burdeos...",
                EsPremium = true,
                FechaCreacion = DateTime.Now
            },
            new Relato
            {
                Id = 3,
                Titulo = "Encuentro en el Jardín Secreto",
                Contenido = "El aroma de las gardenias impregnaba el aire nocturno del jardín escondido tras muros centenarios...",
                EsPremium = false,
                FechaCreacion = DateTime.Now
            }
        };

        // GET: api/relatos
        // Obtener todos los relatos
        [HttpGet]
        public ActionResult<IEnumerable<RelatoDto>> GetRelatos()
        {
            var relatos = _relatos.Select(r => new RelatoDto
            {
                Id = r.Id,
                Titulo = r.Titulo,
                Contenido = r.Contenido,
                EsPremium = r.EsPremium
            }).ToList();

            return Ok(relatos);
        }

        // GET: api/relatos/5
        // Obtener un relato por ID
        [HttpGet("{id}")]
        public ActionResult<RelatoDto> GetRelato(int id)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);

            if (relato == null)
            {
                return NotFound(new { message = "Relato no encontrado" });
            }

            var relatoDto = new RelatoDto
            {
                Id = relato.Id,
                Titulo = relato.Titulo,
                Contenido = relato.Contenido,
                EsPremium = relato.EsPremium
            };

            return Ok(relatoDto);
        }

        // POST: api/relatos
        // Crear un nuevo relato (solo admin - validaremos después)
        [HttpPost]
        public ActionResult<RelatoDto> CreateRelato([FromBody] RelatoDto relatoDto)
        {
            if (string.IsNullOrEmpty(relatoDto.Titulo) || string.IsNullOrEmpty(relatoDto.Contenido))
            {
                return BadRequest(new { message = "El título y contenido son obligatorios" });
            }

            var nuevoRelato = new Relato
            {
                Id = _relatos.Any() ? _relatos.Max(r => r.Id) + 1 : 1,
                Titulo = relatoDto.Titulo,
                Contenido = relatoDto.Contenido,
                EsPremium = relatoDto.EsPremium,
                FechaCreacion = DateTime.Now
            };

            _relatos.Add(nuevoRelato);

            var resultado = new RelatoDto
            {
                Id = nuevoRelato.Id,
                Titulo = nuevoRelato.Titulo,
                Contenido = nuevoRelato.Contenido,
                EsPremium = nuevoRelato.EsPremium
            };

            return CreatedAtAction(nameof(GetRelato), new { id = nuevoRelato.Id }, resultado);
        }

        // PUT: api/relatos/5
        // Actualizar un relato existente (solo admin - validaremos después)
        [HttpPut("{id}")]
        public IActionResult UpdateRelato(int id, [FromBody] RelatoDto relatoDto)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);

            if (relato == null)
            {
                return NotFound(new { message = "Relato no encontrado" });
            }

            if (string.IsNullOrEmpty(relatoDto.Titulo) || string.IsNullOrEmpty(relatoDto.Contenido))
            {
                return BadRequest(new { message = "El título y contenido son obligatorios" });
            }

            relato.Titulo = relatoDto.Titulo;
            relato.Contenido = relatoDto.Contenido;
            relato.EsPremium = relatoDto.EsPremium;
            relato.FechaActualizacion = DateTime.Now;

            return Ok(new
            {
                message = "Relato actualizado correctamente",
                relato = new RelatoDto
                {
                    Id = relato.Id,
                    Titulo = relato.Titulo,
                    Contenido = relato.Contenido,
                    EsPremium = relato.EsPremium
                }
            });
        }

        // DELETE: api/relatos/5
        // Eliminar un relato (solo admin - validaremos después)
        [HttpDelete("{id}")]
        public IActionResult DeleteRelato(int id)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);

            if (relato == null)
            {
                return NotFound(new { message = "Relato no encontrado" });
            }

            _relatos.Remove(relato);

            return Ok(new { message = "Relato eliminado correctamente" });
        }
    }
}