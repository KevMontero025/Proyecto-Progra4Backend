
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BW.Interfaces.BW
{
    public interface ITransaccionCuentaBW
    {
        Task<bool> registrarTransaccion(TransaccionCuenta transaccion);
        Task<TransaccionCuenta> obtenerTransaccion(int id);
        Task<List<TransaccionCuenta>> obtenerTransaccionesPorCuenta(int cuentaId);
        Task<List<TransaccionCuenta>> obtenerTransaccionesPorCliente(int clienteId);

        Task<List<TransaccionCuenta>> obtenerHistorial(
           int? clienteId,
           int? cuentaId,
           DateTime? desde,
           DateTime? hasta,
           TipoOperacionCuenta? tipoOperacion,
           string? estadoOperacion);
    }
}
