

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IProveedorServicioDA
    {
        Task<bool> registrarProveedor(ProveedorServicio proveedor);
        Task<bool> actualizarProveedor(ProveedorServicio proveedor, int id);
        Task<bool> eliminarProveedor(int id);
        Task<ProveedorServicio> obtenerProveedor(int id);
        Task<List<ProveedorServicio>> obtenerProveedores();
    }
}
