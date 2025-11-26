

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface ITransaccionCuentaDA
    {
        Task<bool> registrarTransaccion(TransaccionCuenta transaccion);
        Task<TransaccionCuenta> obtenerTransaccion(int id);
        Task<List<TransaccionCuenta>> obtenerTransaccionesPorCuenta(int cuentaId);
        Task<List<TransaccionCuenta>> obtenerTransaccionesPorCliente(int clienteId);
    }
}
