

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IConsumoLimiteDiarioBW
    {
        Task<ConsumoLimiteDiario> obtenerConsumoPorFecha(int clienteId, DateTime fecha);
        Task<bool> registrarOActualizarConsumo(ConsumoLimiteDiario consumo);
        Task<List<ConsumoLimiteDiario>> obtenerConsumosPorCliente(int clienteId);
    }
}
