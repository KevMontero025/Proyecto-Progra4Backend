using Proyecto.BC.Modelos;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;


namespace Proyecto.BW.CU
{
    public class GestionAuditoriaBW : IAuditoriaBW
    {
        private readonly IAuditoriaDA auditoriaDA;
        public GestionAuditoriaBW(IAuditoriaDA auditoriaDA)
        {
            this.auditoriaDA = auditoriaDA;
        }
        public async Task<bool> registrarLog(LogAuditoria log)
        {
            // Validar que el log tenga datos correctos
            ReglasDeLogAuditoria.Validar(log);
            // Guardar en base de datos
            var resultado = await auditoriaDA.registrarLog(log);

            return resultado;
        }

        public Task<List<LogAuditoria>> obtenerLogs()
        {
            return auditoriaDA.obtenerLogs();
        }

        public Task<List<LogAuditoria>> obtenerLogsPorUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new Exception("El id de usuario para consultar auditoria debe ser mayor a cero");
            return auditoriaDA.obtenerLogsPorUsuario(usuarioId);
        }
    }
}
