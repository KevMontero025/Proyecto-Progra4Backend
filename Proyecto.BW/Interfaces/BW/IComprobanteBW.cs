

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IComprobanteBW
    {
        Task<bool> generarComprobante(Comprobante comprobante);
        Task<Comprobante> obtenerComprobante(int id);
        Task<List<Comprobante>> obtenerComprobantesPorCliente(int clienteId);
    }
}
