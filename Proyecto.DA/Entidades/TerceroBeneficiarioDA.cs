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
    [Table("TerceroBeneficiario")]
    public class TerceroBeneficiarioDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TerceroBeneficiarioId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public string Banco { get; set; }

        [Required]
        public string NumeroCuenta { get; set; }

        [Required]
        public Moneda Moneda { get; set; }

        public string Pais { get; set; }

        [Required]
        public EstadoBeneficiario Estado { get; set; }

        public bool Confirmado { get; set; }
    }
}