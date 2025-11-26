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
    [Table("PagoServicio")]
    public class PagoServicioDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PagoServicioId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int ProveedorServicioId { get; set; }

        [Required]
        public string NumeroContrato { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public Moneda Moneda { get; set; }

        [Required]
        public int CuentaOrigenId { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        [Required]
        public DateTime FechaEjecucion { get; set; }

        public bool EsProgramado { get; set; }

        [Required]
        public EstadoPagoServicio Estado { get; set; }

        public string NumeroReferencia { get; set; }
        public decimal Comision { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }

        [ForeignKey("ProveedorServicioId")]
        public ProveedorServicioDA Proveedor { get; set; }

        [ForeignKey("CuentaOrigenId")]
        public CuentaDA CuentaOrigen { get; set; }
    }
}
