namespace Relatosxxx.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Guardamos el hash, no la clave plana
        public bool IsPremium { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
    }

}