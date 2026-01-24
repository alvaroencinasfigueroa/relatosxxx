using Microsoft.AspNetCore.Mvc;
using Relatosxxx.Models;
using System.Reflection;

namespace Relatosxxx.Models
{
    public class RelatoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public bool EsPremium { get; set; }
    }
}
