using Proyecto.BC.Modelos.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DA.Entidades
{
    [Table("TransaccionCuenta")]
    public class TransaccionCuentaDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransaccionCuentaId { get; set; }

        [Required]
        public int CuentaId { get; set; }

        [ForeignKey("CuentaId")]
        public CuentaDA Cuenta { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TipoTransaccion Tipo { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public decimal SaldoResultante { get; set; }

        public string Descripcion { get; set; }

        public int? ReferenciaInternaId { get; set; } // Id de transferencia o pago
    }
}
