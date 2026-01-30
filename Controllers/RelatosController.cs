using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Relatosxxx.Models;

namespace Relatosxxx.Controllers
{
    // Quitamos [Route("api/...")] para usar la ruta normal de MVC
    public class RelatosController : Controller
    {
        // Tu lista en memoria (NOTA: Si reinicias el programa, esto se borra.
        // Más adelante conectaremos esto a la Base de Datos real).
        private static List<Relato> _relatos = new List<Relato>
        {
            new Relato { Id = 1, Titulo = "Cita bajo la lluvia", Categoria = "romantico", Contenido = "...", EsPremium = false, ImagenUrl = "https://via.placeholder.com/300" },
            new Relato { Id = 2, Titulo = "El secreto del Duque", Categoria = "misterio", Contenido = "...", EsPremium = true, ImagenUrl = "https://via.placeholder.com/300" },
            new Relato { Id = 3, Titulo = "Fuego en la piel", Categoria = "pasion", Contenido = "...", EsPremium = true, ImagenUrl = "https://via.placeholder.com/300" }
        };

        // GET: /Relatos/Create
        // Esta acción MUESTRA el formulario
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Relatos/Create
        // Esta acción RECIBE los datos y guarda el relato
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Relato nuevoRelato)
        {
            if (ModelState.IsValid)
            {
                // Generamos ID automático basado en el último
                nuevoRelato.Id = _relatos.Count > 0 ? _relatos.Max(r => r.Id) + 1 : 1;

                // Si no pone imagen, ponemos una por defecto
                if (string.IsNullOrEmpty(nuevoRelato.ImagenUrl))
                    nuevoRelato.ImagenUrl = "https://via.placeholder.com/300";

                _relatos.Add(nuevoRelato);

                // Redirigimos al inicio para ver el nuevo relato
                return RedirectToAction("Index", "Home");
            }
            return View(nuevoRelato);
        }
    }
}