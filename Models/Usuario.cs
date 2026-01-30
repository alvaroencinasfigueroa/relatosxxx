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

    public class UserDto // Para recibir datos del registro
    {
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class UserLoginDto // Para recibir datos del login
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}