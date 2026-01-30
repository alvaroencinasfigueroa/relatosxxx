using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Relatosxxx.Data;
using Relatosxxx.Models; // Aquí importamos tus clases LoginRequest y RegisterRequest
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // REGISTRO
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Verifica que el usuario no exista
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "El usuario ya existe" });
            }

            // Validamos si es tu correo para darte Admin
            bool esElJefe = request.Email.ToLower() == "alvaro19aef@gmail.com";

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                // Usamos PasswordHash como vimos en tu modelo
                PasswordHash = request.Password,
                IsAdmin = esElJefe, // TRUE solo si eres tú
                IsPremium = false
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = esElJefe ? "¡Bienvenido Admin! Cuenta creada." : "Registro exitoso" });
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Buscamos usuario que coincida en Email y PasswordHash
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciales incorrectas" });
            }

            // Generar Token
            var tokenString = GenerateToken(usuario);

            return Ok(new
            {
                token = tokenString,
                message = "Login exitoso",
                user = new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    email = usuario.Email,
                    isAdmin = usuario.IsAdmin
                }
            });
        }

        // Método auxiliar para crear el token
        private string GenerateToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres123456";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim("id", usuario.Id.ToString()),
                new Claim("role", usuario.IsAdmin ? "Admin" : "User")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}