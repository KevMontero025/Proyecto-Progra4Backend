

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IConsumoLimiteDiarioDA
    {
        Task<ConsumoLimiteDiario> obtenerConsumoPorFecha(int clienteId, DateTime fecha);
        Task<bool> registrarOActualizarConsumo(ConsumoLimiteDiario consumo);
        Task<List<ConsumoLimiteDiario>> obtenerConsumosPorCliente(int clienteId);
    }
}
