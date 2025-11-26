using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Proyecto.BW.CU
{
    public class GestionAutenticacionBW : IAutenticacionBW
    {
        private readonly IUsuarioDA usuarioDA;
        private readonly IConfiguration configuration;

        public GestionAutenticacionBW(IUsuarioDA usuarioDA, IConfiguration configuration)
        {
            this.usuarioDA = usuarioDA;
            this.configuration = configuration;
        }

        public async Task<string> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception("El email y la contraseña son obligatorios");

            var usuario = await usuarioDA.obtenerPorCorreo(email);

            if (usuario == null)
                throw new Exception("Credenciales inválidas");

            if (usuario.IsLockedOut && usuario.LockoutEnd.HasValue && usuario.LockoutEnd > DateTime.Now)
            {
                throw new Exception($"Usuario bloqueado hasta {usuario.LockoutEnd.Value:dd/MM/yyyy HH:mm}");
            }
            var passwordEsCorrecta = usuario.PasswordHash == password;

            if (!passwordEsCorrecta)
            {
                usuario.IntentosFallidosLogin++;

                if (usuario.IntentosFallidosLogin >= 5)
                {
                    usuario.IsLockedOut = true;
                    usuario.LockoutEnd = DateTime.Now.AddMinutes(15);
                }

                await usuarioDA.actualizarUsuario(usuario, usuario.UsuarioId);

                throw new Exception("Credenciales inválidas");
            }

            usuario.IntentosFallidosLogin = 0;
            usuario.IsLockedOut = false;
            usuario.LockoutEnd = null;

            await usuarioDA.actualizarUsuario(usuario, usuario.UsuarioId);

            return GenerarJwt(usuario);
        }

        private string GenerarJwt(Usuario usuario)
        {
            var key = configuration["Jwt:Key"];
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(key))
                throw new Exception("No se ha configurado Jwt:Key en appsettings.json");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UsuarioId", usuario.UsuarioId.ToString()),
                new Claim("Email", usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
