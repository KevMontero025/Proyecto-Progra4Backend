using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionTransferenciaDA : ITransferenciaDA
    {
        private readonly BancoContext bancoContext;

        public GestionTransferenciaDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> cancelarTransferencia(int id)
        {
            var transferencia = await bancoContext.Transferencia.FindAsync(id);
            if (transferencia == null)
                return false;

            transferencia.Estado = EstadoTransferencia.Cancelada;
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> crearTransferencia(Transferencia transferencia)
        {
            await using var transaction = await bancoContext.Database.BeginTransactionAsync();
            try
            {
                await bancoContext.Transferencia.AddAsync(transferencia);
                await bancoContext.SaveChangesAsync();
                await transaction.CommitAsync(); 
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Error al crear la transferencia: " + ex.Message);
            }
        }
        public async Task<bool> actualizarTransferencia(Transferencia transferencia)
        {
            bancoContext.Transferencia.Update(transferencia);
            await bancoContext.SaveChangesAsync();
            return true;
        }
        public Task<Transferencia> obtenerTransferencia(int id)
        {
            return bancoContext.Transferencia
                .FirstAsync(t => t.TransferenciaId == id);
        }

        public Task<List<Transferencia>> obtenerTransferenciasPorCliente(int clienteId)
        {
            return bancoContext.Transferencia
                .Where(t => t.ClienteId == clienteId)
                .ToListAsync();
        }

        public Task<List<Transferencia>> obtenerTransferenciasProgramadasPorBeneficiario(int terceroBeneficiarioId)
        {
            // Aqui se busca todas las transferencias que, tienen ese beneficiario como destino, estan marcadas como programadas
            //su fecha de ejecucion es futura y el estado es programada o pendiente de aprobacion
            return bancoContext.Transferencia
                .Where(t => t.TerceroBeneficiarioId == terceroBeneficiarioId
                            && t.EsProgramada
                            && t.FechaEjecucion > DateTime.Now
                            && (t.Estado == EstadoTransferencia.Programada
                                || t.Estado == EstadoTransferencia.PendienteAprobacion))
                .ToListAsync();
        }
        public Task<List<Transferencia>> obtenerTransferenciasProgramadasParaEjecutar()
        {
            return bancoContext.Transferencia
                .Where(t => t.EsProgramada
                            && t.FechaEjecucion <= DateTime.Now
                            && (t.Estado == EstadoTransferencia.Programada
                                || t.Estado == EstadoTransferencia.PendienteAprobacion))
                .ToListAsync();
        }
        public Task<Transferencia?> obtenerPorIdempotencyKey(string idempotencyKey)
        {
            return bancoContext.Transferencia
                .FirstOrDefaultAsync(t => t.IdempotencyKey == idempotencyKey);
        }
    }
}
