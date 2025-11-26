

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.DA.Entidades
{
    [Table("Comprobante")]
    public class ComprobanteDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComprobanteId { get; set; }

        [Required]
        public string NumeroReferencia { get; set; }

        [Required]
        public string Tipo { get; set; } // Transferencia / PagoServicio

        [Required]
        public int OperacionId { get; set; } // Id de la operacion asociada

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int CuentaId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public string RutaArchivo { get; set; }
        public string DatosJson { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }

        [ForeignKey("CuentaId")]
        public CuentaDA Cuenta { get; set; }
    }
}
