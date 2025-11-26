

using Proyecto.BC.Modelos;
using System.Text.RegularExpressions;

namespace Proyecto.BC.ReglasDeNegocios
{
    public static class ReglasDeUsuario
    {
        public static void Validar(Usuario usuario)
        {
            if (usuario == null)
                throw new Exception("El usuario no puede ser null");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                throw new Exception("El email del usuario es obligatorio");

            if (!EsEmailValido(usuario.Email))
                throw new Exception("El email del usuario no tiene un formato valido");

            if (string.IsNullOrWhiteSpace(usuario.PasswordHash))
                throw new Exception("La contrasena del usuario no puede ir vacia");

        }

        // Esta funcion valida la politica de contrasena sobre el texto plano
        public static void ValidarPasswordPlano(string passwordPlano)
        {
            if (string.IsNullOrWhiteSpace(passwordPlano))
                throw new Exception("La contrasena no puede ir vacia");

            if (passwordPlano.Length < 8)
                throw new Exception("La contrasena debe tener al menos 8 caracteres");

            if (!passwordPlano.Any(char.IsUpper))
                throw new Exception("La contrasena debe tener al menos una letra mayuscula");

            if (!passwordPlano.Any(char.IsDigit))
                throw new Exception("La contrasena debe tener al menos un numero");

            if (!passwordPlano.Any(c => !char.IsLetterOrDigit(c)))
                throw new Exception("La contrasena debe tener al menos un simbolo");
        }

        private static bool EsEmailValido(string email)
        {
            // Patron simple: algo@algo.algo
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de usuario debe ser mayor a cero");
        }
    }
}
