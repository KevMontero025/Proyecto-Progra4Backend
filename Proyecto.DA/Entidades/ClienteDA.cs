

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.DA.Entidades
{
    [Table("Cliente")]
    public class ClienteDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClienteId { get; set; }

        [Required]
        public string NombreCompleto { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public UsuarioDA Usuario { get; set; }

        // Un cliente puede tener muchos beneficiarios, muchas cuentas y muchos consumos diarios
        public ICollection<CuentaDA> Cuentas { get; set; }
        public ICollection<TerceroBeneficiarioDA> Beneficiarios { get; set; }
        public ICollection<ConsumoLimiteDiarioDA> Consumos { get; set; }
    }
}
