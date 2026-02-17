using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Relatosxxx.Data;
using Relatosxxx.Models;
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
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Email y contraseña son requeridos" });
            }

            if (request.Password.Length < 6)
            {
                return BadRequest(new { message = "La contraseña debe tener al menos 6 caracteres" });
            }

            // Verifica que el usuario no exista
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest(new { message = "El usuario ya existe" });
            }

            // Validamos si es el correo de admin configurado
            var adminEmail = _configuration["AdminEmail"]?.ToLower();
            bool esElJefe = !string.IsNullOrEmpty(adminEmail) &&
                           request.Email.ToLower() == adminEmail;

            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Email = request.Email,
                // CRÍTICO: Hasheamos la contraseña antes de guardarla
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsAdmin = esElJefe,
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
            // Buscar usuario por email
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Si no existe el usuario
            if (usuario == null)
            {
                return Unauthorized(new { message = "Email o contraseña incorrectos" });
            }

            // CRÍTICO: Verificamos la contraseña hasheada
            bool passwordValida = BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash);

            if (!passwordValida)
            {
                return Unauthorized(new { message = "Email o contraseña incorrectos" });
            }

            // Generar token JWT
            var token = GenerateJwtToken(usuario);

            return Ok(new
            {
                token,
                user = new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    email = usuario.Email,
                    isPremium = usuario.IsPremium,
                    isAdmin = usuario.IsAdmin
                }
            });
        }

        // ACTIVAR PREMIUM (para usar después de confirmar el pago)
        [HttpPost("activate-premium")]
        public async Task<IActionResult> ActivatePremium([FromBody] ActivatePremiumRequest request)
        {
            // Buscar usuario por email o ID
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email || u.Id == request.UserId);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            if (usuario.IsPremium)
            {
                return Ok(new { message = "El usuario ya es Premium" });
            }

            // Activar premium
            usuario.IsPremium = true;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Suscripción Premium activada con éxito",
                user = new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    email = usuario.Email,
                    isPremium = usuario.IsPremium
                }
            });
        }

        // Método auxiliar para generar el token
        private string GenerateJwtToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"] ?? "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres123456"
            ));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.IsAdmin ? "Admin" : "User"),
                new Claim("IsPremium", usuario.IsPremium.ToString())
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