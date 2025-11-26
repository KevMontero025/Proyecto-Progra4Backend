
using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IClienteDA
    {
        Task<bool> registrarCliente(Cliente cliente);
        Task<bool> actualizarCliente(Cliente cliente, int id);
        Task<bool> eliminarCliente(int id);
        Task<Cliente> obtenerCliente(int id);
        Task<List<Cliente>> obtenerClientes();

        // Metodos extra solo para validaciones internas
        Task<Cliente?> obtenerPorIdentificacion(string identificacion);
        Task<Cliente?> obtenerPorCorreo(string correo);
        Task<Cliente?> obtenerPorUsuarioId(int usuarioId);
    }
}
