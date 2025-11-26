using Proyecto.BC.Modelos;


namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar un registro de auditoria.
    public static class ReglasDeLogAuditoria
    {
        public static void Validar(LogAuditoria log)
        {
            if (log == null)
                throw new Exception("El registro de auditoria no puede ser null");

            if (log.UsuarioId <= 0)
                throw new Exception("El registro de auditoria debe tener un usuario valido");

            if (log.Fecha == default(DateTime))
                throw new Exception("La fecha del registro de auditoria no es valida");

            if (string.IsNullOrWhiteSpace(log.Entidad))
                throw new Exception("La entidad afectada es obligatoria en auditoria");

            if (string.IsNullOrWhiteSpace(log.EntidadId))
                throw new Exception("El id de la entidad afectada es obligatorio en auditoria");
        }

    }
}
