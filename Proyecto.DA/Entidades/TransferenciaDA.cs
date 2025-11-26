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
    [Table("Transferencia")]
    public class TransferenciaDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransferenciaId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int CuentaOrigenId { get; set; }

        public int? CuentaDestinoId { get; set; }

        public int? TerceroBeneficiarioId { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public Moneda Moneda { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaEjecucion { get; set; }

        public bool EsProgramada { get; set; }

        [Required]
        public EstadoTransferencia Estado { get; set; }

        public decimal SaldoAntes { get; set; }
        public decimal SaldoDespues { get; set; }
        public decimal Comision { get; set; }

        [Required]
        public string IdempotencyKey { get; set; }

        public bool NecesitaAprobacion { get; set; }
        public int? AprobadaPorUsuarioId { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }

        [ForeignKey("CuentaOrigenId")]
        public CuentaDA CuentaOrigen { get; set; }

        [ForeignKey("CuentaDestinoId")]
        public CuentaDA CuentaDestino { get; set; }

        [ForeignKey("TerceroBeneficiarioId")]
        public TerceroBeneficiarioDA TerceroBeneficiario { get; set; }

        [ForeignKey("AprobadaPorUsuarioId")]
        public UsuarioDA UsuarioAprobador { get; set; }
    }
}
