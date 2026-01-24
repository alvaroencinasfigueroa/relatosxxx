using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Relatosxxx.Models;
//using BCrypt.Net;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Lista temporal de usuarios (después usaremos BD)
        private static List<Usuario> _usuarios = new List<Usuario>
        {
            new Usuario
            {
                Id = 1,
                Nombre = "Admin",
                Email = "admin@relatos.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                IsAdmin = true,
                IsPremium = true,
                FechaRegistro = DateTime.Now
            }
        };

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public ActionResult<object> Register([FromBody] RegisterRequest request)
        {
            // Validaciones
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email y contraseña son obligatorios" });
            }

            if (request.Password.Length < 6)
            {
                return BadRequest(new { message = "La contraseña debe tener al menos 6 caracteres" });
            }

            // Verificar si el email ya existe
            if (_usuarios.Any(u => u.Email.ToLower() == request.Email.ToLower()))
            {
                return BadRequest(new { message = "Este email ya está registrado" });
            }

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Id = _usuarios.Any() ? _usuarios.Max(u => u.Id) + 1 : 1,
                Nombre = request.Nombre,
                Email = request.Email.ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsPremium = true, // Por ahora todos son premium (después implementaremos pago)
                IsAdmin = false,
                FechaRegistro = DateTime.Now,
                FechaSuscripcionPremium = DateTime.Now
            };

            _usuarios.Add(nuevoUsuario);

            return Ok(new
            {
                message = "Usuario registrado exitosamente",
                user = new
                {
                    id = nuevoUsuario.Id,
                    nombre = nuevoUsuario.Nombre,
                    email = nuevoUsuario.Email,
                    isPremium = nuevoUsuario.IsPremium,
                    isAdmin = nuevoUsuario.IsAdmin
                }
            });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public ActionResult<object> Login([FromBody] LoginRequest request)
        {
            // Validaciones
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Email y contraseña son obligatorios" });
            }

            // Buscar usuario
            var usuario = _usuarios.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());

            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            // Verificar contraseña
            if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            // Generar token JWT
            var token = GenerateJwtToken(usuario);

            return Ok(new
            {
                message = "Login exitoso",
                token = token,
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

        // Método privado para generar el token JWT
        private string GenerateJwtToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres123456"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("IsPremium", usuario.IsPremium.ToString()),
                new Claim("IsAdmin", usuario.IsAdmin.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "RelatosApp",
                audience: _configuration["Jwt:Audience"] ?? "RelatosApp",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET: api/auth/usuarios (solo para pruebas - eliminar en producción)
        [HttpGet("usuarios")]
        public ActionResult<object> GetUsuarios()
        {
            return Ok(_usuarios.Select(u => new
            {
                id = u.Id,
                nombre = u.Nombre,
                email = u.Email,
                isPremium = u.IsPremium,
                isAdmin = u.IsAdmin
            }));
        }
    }
}