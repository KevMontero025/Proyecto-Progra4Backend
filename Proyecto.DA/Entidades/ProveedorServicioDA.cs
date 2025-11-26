using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DA.Entidades
{
    [Table("ProveedorServicio")]
    public class ProveedorServicioDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProveedorServicioId { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int LongitudMinContrato { get; set; }

        [Required]
        public int LongitudMaxContrato { get; set; }

        public string ReglasAdicionales { get; set; }

        public ICollection<PagoServicioDA> Pagos { get; set; }
    }
}
