

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface ITransferenciaBW
    {
        Task<bool> crearTransferencia(Transferencia transferencia, int usuarioAccionId);
        Task<bool> cancelarTransferencia(int id, int usuarioAccionId);
        Task<bool> aprobarTransferencia(int id, int usuarioAdminId);
        Task<bool> rechazarTransferencia(int id, int usuarioAdminId);
        Task<Transferencia> obtenerTransferencia(int id);
        Task<List<Transferencia>> obtenerTransferenciasPorCliente(int clienteId);
    }
}
