
using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.Modelos
{
   // Representa a un usuario del sistema
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } // Contraseña encriptada
        public RolUsuario Rol { get; set; }
        public int IntentosFallidosLogin { get; set; } // Para bloqueo por intentos
        public bool IsLockedOut { get; set; }          // Si el usuario esta bloqueado
        public DateTime? LockoutEnd { get; set; }      // Hasta cuando esta bloqueado
    }
}
