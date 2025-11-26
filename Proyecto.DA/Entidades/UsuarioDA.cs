

using Proyecto.BC.Modelos.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.DA.Entidades
{
    [Table("Usuario")]
    public class UsuarioDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public RolUsuario Rol { get; set; }

        public int IntentosFallidosLogin { get; set; }
        public bool IsLockedOut { get; set; }
        public DateTime? LockoutEnd { get; set; }

        //Colecciones de clientes, auditorias, etc.
        public ICollection<ClienteDA> Clientes { get; set; }
        public ICollection<LogAuditoriaDA> Logs { get; set; }
    }
}
