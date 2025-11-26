using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DA.Entidades
{
    [Table("ConsumoLimiteDiario")]
    public class ConsumoLimiteDiarioDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsumoLimiteDiarioId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal MontoTotalTransferido { get; set; }
    }
}
