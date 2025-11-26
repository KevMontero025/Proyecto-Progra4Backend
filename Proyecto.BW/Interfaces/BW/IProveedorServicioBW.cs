

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IProveedorServicioBW
    {
        Task<bool> registrarProveedor(ProveedorServicio proveedor, int usuarioAccionId);
        Task<bool> actualizarProveedor(ProveedorServicio proveedor, int id, int usuarioAccionId);
        Task<bool> eliminarProveedor(int id, int usuarioAccionId);
        Task<ProveedorServicio> obtenerProveedor(int id);
        Task<List<ProveedorServicio>> obtenerProveedores();
    }
}
