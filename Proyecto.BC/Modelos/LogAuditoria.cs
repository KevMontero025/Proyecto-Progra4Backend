using Proyecto.BC.Modelos.Enum;


namespace Proyecto.BC.Modelos
{
    // Registro de acciones importantes del sistema
    public class LogAuditoria
    {
        public int LogAuditoriaId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public TipoOperacionAuditoria TipoOperacion { get; set; } // Que tipo de accion
        public string Entidad { get; set; } // Que entidad se afecto
        public string EntidadId { get; set; } //Id de esa entidad
        public string ValoresAnteriores { get; set; } //Antes del cambio
        public string ValoresNuevos { get; set; } //Despues del cambio
    }
}
