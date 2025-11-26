

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IComprobanteDA
    {
        Task<bool> generarComprobante(Comprobante comprobante);
        Task<Comprobante> obtenerComprobante(int id);
        Task<List<Comprobante>> obtenerComprobantesPorCliente(int clienteId);
    }
}
