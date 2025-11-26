

using Proyecto.BC.Modelos.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.DA.Entidades
{
    [Table("Cuenta")]
    public class CuentaDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CuentaId { get; set; }

        [Required]
        public string NumeroCuenta { get; set; }

        [Required]
        public TipoCuenta TipoCuenta { get; set; }

        [Required]
        public Moneda Moneda { get; set; }

        [Required]
        public decimal Saldo { get; set; }

        [Required]
        public EstadoCuenta EstadoCuenta { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }

        public ICollection<TransaccionCuentaDA> Transacciones { get; set; }
    }
}
