

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IGestorClienteDA
    {
        Task<bool> asignarGestorACliente(GestorCliente relacion);
        Task<bool> eliminarAsignacion(int gestorId, int clienteId);
        Task<List<GestorCliente>> obtenerClientesPorGestor(int gestorId);
        Task<List<GestorCliente>> obtenerGestoresPorCliente(int clienteId);
    }
}
