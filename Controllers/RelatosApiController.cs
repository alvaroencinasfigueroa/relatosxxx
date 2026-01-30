using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Relatosxxx.Models;
using System.Collections.Generic;
using System.Linq;

namespace Relatosxxx.Controllers
{
    [Route("api/Relatos")]
    [ApiController]
    public class RelatosApiController : ControllerBase
    {
        // Lista estática compartida con RelatosController
        private static List<Relato> _relatos = new List<Relato>
        {
            new Relato { Id = 1, Titulo = "Cita bajo la lluvia", Categoria = "romantico", Contenido = "Una noche de lluvia, dos almas se encuentran bajo el mismo paraguas...", EsPremium = false, ImagenUrl = "https://via.placeholder.com/400x200/ff69b4/ffffff?text=Romance" },
            new Relato { Id = 2, Titulo = "El secreto del Duque", Categoria = "misterio", Contenido = "En las sombras de su mansión, el Duque esconde un secreto que podría cambiar todo...", EsPremium = true, ImagenUrl = "https://via.placeholder.com/400x200/4b0082/ffffff?text=Misterio" },
            new Relato { Id = 3, Titulo = "Fuego en la piel", Categoria = "pasion", Contenido = "Sus miradas se cruzaron y el mundo se detuvo. La pasión era inevitable...", EsPremium = true, ImagenUrl = "https://via.placeholder.com/400x200/dc143c/ffffff?text=Pasion" }
        };

        // GET: api/RelatosApi
        [HttpGet]
        public ActionResult<IEnumerable<Relato>> GetAll()
        {
            return Ok(_relatos);
        }

        // GET: api/RelatosApi/5
        [HttpGet("{id}")]
        public ActionResult<Relato> GetById(int id)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            return Ok(relato);
        }

        // POST: api/RelatosApi
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<Relato> Create([FromBody] Relato nuevoRelato)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Generar ID automático
            nuevoRelato.Id = _relatos.Count > 0 ? _relatos.Max(r => r.Id) + 1 : 1;

            // Imagen por defecto si no se proporciona
            if (string.IsNullOrEmpty(nuevoRelato.ImagenUrl))
                nuevoRelato.ImagenUrl = "https://via.placeholder.com/400x200/ff69b4/ffffff?text=Relato";

            _relatos.Add(nuevoRelato);

            return CreatedAtAction(nameof(GetById), new { id = nuevoRelato.Id }, nuevoRelato);
        }

        // PUT: api/RelatosApi/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, [FromBody] Relato relatoActualizado)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            // Actualizar propiedades
            relato.Titulo = relatoActualizado.Titulo;
            relato.Categoria = relatoActualizado.Categoria;
            relato.Contenido = relatoActualizado.Contenido;
            relato.ImagenUrl = relatoActualizado.ImagenUrl;
            relato.EsPremium = relatoActualizado.EsPremium;

            return Ok(relato);
        }

        // DELETE: api/RelatosApi/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var relato = _relatos.FirstOrDefault(r => r.Id == id);
            if (relato == null)
                return NotFound(new { message = "Relato no encontrado" });

            _relatos.Remove(relato);

            return Ok(new { message = "Relato eliminado exitosamente" });
        }
    }
}