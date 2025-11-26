using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DA.Entidades
{
    [Table("ParametroSistema")]
    public class ParametroSistemaDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ParametroSistemaId { get; set; }

        [Required]
        public string Clave { get; set; }

        [Required]
        public string Valor { get; set; }

        public string Descripcion { get; set; }
    }
}
