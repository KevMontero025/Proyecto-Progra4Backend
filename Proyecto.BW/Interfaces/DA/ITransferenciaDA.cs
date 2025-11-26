

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface ITransferenciaDA
    {
        Task<bool> crearTransferencia(Transferencia transferencia);
        Task<bool> cancelarTransferencia(int id);
        Task<Transferencia> obtenerTransferencia(int id);
        Task<List<Transferencia>> obtenerTransferenciasPorCliente(int clienteId);
        Task<bool> actualizarTransferencia(Transferencia transferencia);
        Task<List<Transferencia>> obtenerTransferenciasProgramadasPorBeneficiario(int terceroBeneficiarioId);
        Task<List<Transferencia>> obtenerTransferenciasProgramadasParaEjecutar();
        Task<Transferencia?> obtenerPorIdempotencyKey(string idempotencyKey);

    }
}
