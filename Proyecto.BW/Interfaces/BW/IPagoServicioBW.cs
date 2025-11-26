

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IPagoServicioBW
    {
        Task<bool> realizarPago(PagoServicio pago, int usuarioAccionId);
        Task<bool> cancelarPago(int id, int usuarioAccionId);
        Task<PagoServicio> obtenerPago(int id);
        Task<List<PagoServicio>> obtenerPagosPorCliente(int clienteId);
    }
}
