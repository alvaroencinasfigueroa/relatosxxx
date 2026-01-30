using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Relatosxxx.Models;

namespace Relatosxxx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Variable estática para simular la base de datos en memoria (se reinicia si detienes el servidor)
        private static List<Usuario> _usuarios = new List<Usuario>();

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<Usuario> Register(UserDto request)
        {
            // 1. Validar si el usuario ya existe
            if (_usuarios.Any(u => u.Email.ToLower() == request.Email.ToLower()))
            {
                return BadRequest(new { message = "El usuario ya existe." });
            }

            // 2. Crear el hash de la contraseña
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Crear el nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Id = _usuarios.Count + 1,
                Nombre = request.Nombre,
                Email = request.Email,
                PasswordHash = passwordHash,

                // --- AQUÍ ESTÁ EL CAMBIO ---
                // Solo tu correo será Administrador. 
                // Trim() elimina espacios accidentales al inicio o final.
                IsAdmin = request.Email.Trim().ToLower() == "alvaro19aef@gmail.com",

                // Por defecto damos Premium a todos para que prueben la app
                IsPremium = true
            };

            _usuarios.Add(nuevoUsuario);

            return Ok(new { message = "Usuario registrado con éxito. Ahora inicia sesión." });
        }

        [HttpPost("login")]
        public ActionResult<object> Login(UserLoginDto request)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());

            if (usuario == null)
            {
                return BadRequest(new { message = "Usuario no encontrado." });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            {
                return BadRequest(new { message = "Contraseña incorrecta." });
            }

            string token = CreateToken(usuario);

            // Retornamos el token y los datos del usuario
            return Ok(new
            {
                token = token,
                message = usuario.IsAdmin ? "👑 Bienvenido Creador" : "Login exitoso",
                user = new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Email,
                    usuario.IsPremium,
                    usuario.IsAdmin
                }
            });
        }

        private string CreateToken(Usuario usuario)
        {
            var keyString = _configuration["Jwt:Key"] ?? "TuClaveSecretaSuperSeguraDeAlMenos32Caracteres123456";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim("IsAdmin", usuario.IsAdmin.ToString()),
                new Claim("IsPremium", usuario.IsPremium.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "RelatosApp",
                audience: _configuration["Jwt:Audience"] ?? "RelatosApp",
                claims: claims,
                expires: DateTime.Now.AddDays(7), // Token válido por 7 días
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}