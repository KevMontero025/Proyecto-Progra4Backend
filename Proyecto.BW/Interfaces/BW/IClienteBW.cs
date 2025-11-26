

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IClienteBW
    {
        Task<bool> registrarCliente(Cliente cliente);
        Task<bool> actualizarCliente(Cliente cliente, int id);
        Task<bool> eliminarCliente(int id);
        Task<Cliente> obtenerCliente(int id);
        Task<List<Cliente>> obtenerClientes();
        Task<Cliente?> obtenerPorCorreo(string correo);
        Task<Cliente?> obtenerPorIdentificacion(string identificacion);
        Task<Cliente?> obtenerPorUsuarioId(int usuarioId);
    }
}
