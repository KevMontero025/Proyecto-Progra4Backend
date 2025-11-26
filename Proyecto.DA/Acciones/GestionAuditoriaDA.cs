using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;
using Microsoft.EntityFrameworkCore;


namespace Proyecto.DA.Acciones
{
    public class GestionAuditoriaDA : IAuditoriaDA
    {
        private readonly BancoContext bancoContext;

        public GestionAuditoriaDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public Task<List<LogAuditoria>> obtenerLogs()
        {
            return bancoContext.LogAuditoria.OrderByDescending(l => l.Fecha).ToListAsync();
        }

        public Task<List<LogAuditoria>> obtenerLogsPorUsuario(int usuarioId)
        {
            // Filtra por usuario
            return bancoContext.LogAuditoria
                .Where(l => l.UsuarioId == usuarioId)
                .OrderByDescending(l => l.Fecha)
                .ToListAsync();
        }

        public async Task<bool> registrarLog(LogAuditoria log)
        {
            try
            {
                bancoContext.LogAuditoria.Add(log);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el log de auditoria: " + ex.Message);
            }
        }
    }
}