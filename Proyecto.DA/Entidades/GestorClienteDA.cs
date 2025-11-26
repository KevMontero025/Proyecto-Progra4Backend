using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.DA.Entidades
{
    [Table("GestorCliente")]
    public class GestorClienteDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GestorClienteId { get; set; }

        [Required]
        public int GestorId { get; set; }  // UsuarioId del gestor

        [Required]
        public int ClienteId { get; set; }

        [ForeignKey("GestorId")]
        public UsuarioDA Gestor { get; set; }

        [ForeignKey("ClienteId")]
        public ClienteDA Cliente { get; set; }
    }
}
