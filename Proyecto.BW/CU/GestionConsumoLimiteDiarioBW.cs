

using Proyecto.BC.Modelos;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;

namespace Proyecto.BW.CU
{
    public class GestionConsumoLimiteDiarioBW : IConsumoLimiteDiarioBW
    {
        private readonly IConsumoLimiteDiarioDA consumoDA;

        public GestionConsumoLimiteDiarioBW(IConsumoLimiteDiarioDA consumoDA)
        {
            this.consumoDA = consumoDA;
        }

        public Task<ConsumoLimiteDiario> obtenerConsumoPorFecha(int clienteId, DateTime fecha)
        {
            ReglasDeCliente.ValidarId(clienteId);

            // Validar que la fecha no sea default
            if (fecha == default(DateTime))
                throw new Exception("La fecha ingresada no es valida");

            // Validar que la fecha no sea una fecha en el futuro
            if (fecha.Date > DateTime.Now.Date)
                throw new Exception("No se puede consultar consumo en fechas futuras");

            return consumoDA.obtenerConsumoPorFecha(clienteId, fecha);
        }

        public Task<List<ConsumoLimiteDiario>> obtenerConsumosPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return consumoDA.obtenerConsumosPorCliente(clienteId);
        }

        public async Task<bool> registrarOActualizarConsumo(ConsumoLimiteDiario consumo)
        {
            ReglasDeConsumoLimiteDiario.Validar(consumo);
            return await consumoDA.registrarOActualizarConsumo(consumo);
        }
    }
}
