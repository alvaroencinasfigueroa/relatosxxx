namespace Relatosxxx.Models
{
    public class Relato
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Contenido { get; set; } = string.Empty;
        public bool EsPremium { get; set; }
    }
}