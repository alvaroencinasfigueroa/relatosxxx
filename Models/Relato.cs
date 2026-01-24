namespace Relatosxxx.Models
{
    public class Relato
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Contenido { get; set; } = string.Empty;

        public bool EsPremium { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaActualizacion { get; set; }

        // Relación con el usuario que lo creó (admin)
        public int? CreadoPorId { get; set; }
    }
}