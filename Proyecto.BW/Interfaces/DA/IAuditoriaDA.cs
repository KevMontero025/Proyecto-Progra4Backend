using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IAuditoriaDA
    {
        Task<bool> registrarLog(LogAuditoria log);
        Task<List<LogAuditoria>> obtenerLogs();
        Task<List<LogAuditoria>> obtenerLogsPorUsuario(int usuarioId);
    }
}
