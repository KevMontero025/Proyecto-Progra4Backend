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
    [Table("LogAuditoria")]
    public class LogAuditoriaDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogAuditoriaId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public UsuarioDA Usuario { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public TipoOperacionAuditoria TipoOperacion { get; set; }

        [Required]
        public string Entidad { get; set; }

        [Required]
        public string EntidadId { get; set; }

        public string ValoresAnteriores { get; set; }
        public string ValoresNuevos { get; set; }
    }
}
