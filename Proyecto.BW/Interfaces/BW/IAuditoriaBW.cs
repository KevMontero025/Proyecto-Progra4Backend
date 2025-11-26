

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IAuditoriaBW
    {
        Task<bool> registrarLog(LogAuditoria log);
        Task<List<LogAuditoria>> obtenerLogs();
        Task<List<LogAuditoria>> obtenerLogsPorUsuario(int usuarioId);
    }
}
