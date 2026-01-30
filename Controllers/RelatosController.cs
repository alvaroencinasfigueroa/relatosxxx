using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Necesario para [Authorize]
using Relatosxxx.Models;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatosController : ControllerBase
    {
        // Simulamos base de datos de relatos en memoria
        private static List<Relato> _relatos = new List<Relato>
        {
            new Relato { Id = 1, Titulo = "Cita bajo la lluvia", Categoria = "romantico", Contenido = "Las gotas caían suavemente...", EsPremium = false },
            new Relato { Id = 2, Titulo = "El secreto del Duque", Categoria = "misterio", Contenido = "Nadie sabía lo que ocurría en la mansión...", EsPremium = true },
            new Relato { Id = 3, Titulo = "Fuego en la piel", Categoria = "pasion", Contenido = "Un roce accidental encendió la llama...", EsPremium = true }
        };

        // GET: api/relatos
        [HttpGet]
        public ActionResult<List<Relato>> GetRelatos()
        {
            return Ok(_relatos);
        }

        // GET: api/relatos/categoria/romantico
        [HttpGet("categoria/{categoria}")]
        public ActionResult<List<Relato>> GetPorCategoria(string categoria)
        {
            var filtrados = _relatos.Where(r => r.Categoria.ToLower() == categoria.ToLower()).ToList();
            return Ok(filtrados);
        }

        // GET: api/relatos/5
        [HttpGet("{id}")]
        public ActionResult<Relato> GetRelato(int id)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            return Ok(relato);
        }

        // POST: api/relatos (Protegido: Requiere Login)
        [HttpPost]
        [Authorize]
        public ActionResult<Relato> CreateRelato(Relato nuevoRelato)
        {
            nuevoRelato.Id = _relatos.Count > 0 ? _relatos.Max(r => r.Id) + 1 : 1;
            _relatos.Add(nuevoRelato);
            return Ok(nuevoRelato); // Devolvemos el relato creado
        }

        // PUT: api/relatos/5 (Protegido: Requiere Login)
        [HttpPut("{id}")]
        [Authorize]
        public ActionResult UpdateRelato(int id, Relato relatoActualizado)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            relato.Titulo = relatoActualizado.Titulo;
            relato.Contenido = relatoActualizado.Contenido;
            relato.Categoria = relatoActualizado.Categoria;
            relato.EsPremium = relatoActualizado.EsPremium;

            return Ok(new { message = "Relato actualizado correctamente" });
        }

        // DELETE: api/relatos/5 (Protegido: Requiere Login)
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteRelato(int id)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            _relatos.Remove(relato);
            return Ok(new { message = "Relato eliminado" });
        }
    }
}