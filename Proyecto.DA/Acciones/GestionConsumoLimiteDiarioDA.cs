using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;
using Microsoft.EntityFrameworkCore;


namespace Proyecto.DA.Acciones
{
    public class GestionConsumoLimiteDiarioDA : IConsumoLimiteDiarioDA
    {
        private readonly BancoContext bancoContext;

        public GestionConsumoLimiteDiarioDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public Task<ConsumoLimiteDiario> obtenerConsumoPorFecha(int clienteId, DateTime fecha)
        {
            // Busca el registro de consumo de ese cliente en esa fecha
            return bancoContext.ConsumoLimiteDiario
                .FirstAsync(c => c.ClienteId == clienteId && c.Fecha.Date == fecha.Date);
        }

        public Task<List<ConsumoLimiteDiario>> obtenerConsumosPorCliente(int clienteId)
        {
            // Devuelve todo el historial de consumos del cliente
            return bancoContext.ConsumoLimiteDiario
                .Where(c => c.ClienteId == clienteId)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }

        public async Task<bool> registrarOActualizarConsumo(ConsumoLimiteDiario consumo)
        {
            // Busca si ya existe un registro para ese cliente y esa fecha
            var existente = await bancoContext.ConsumoLimiteDiario
                .FirstOrDefaultAsync(c => c.ClienteId == consumo.ClienteId &&
                                          c.Fecha.Date == consumo.Fecha.Date);

            if (existente == null)
            {
                // No existe, se crea uno nuevo
                bancoContext.ConsumoLimiteDiario.Add(consumo);
            }
            else
            {
                // Ya existe, se actualiza el monto
                existente.MontoTotalTransferido = consumo.MontoTotalTransferido;
            }

            await bancoContext.SaveChangesAsync();
            return true;
        }
    }
}
