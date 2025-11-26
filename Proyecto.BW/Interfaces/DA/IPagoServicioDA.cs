

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IPagoServicioDA
    {
        Task<bool> realizarPago(PagoServicio pago);
        Task<bool> cancelarPago(int id);
        Task<PagoServicio> obtenerPago(int id);
        Task<List<PagoServicio>> obtenerPagosPorCliente(int clienteId);
    }
}
