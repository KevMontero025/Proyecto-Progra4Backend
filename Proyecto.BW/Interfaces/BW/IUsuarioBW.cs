
using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IUsuarioBW
    {
        Task<bool> registrarUsuario(Usuario usuario, int usuarioEjecutorId);
        Task<bool> actualizarUsuario(Usuario usuario, int id, int usuarioEjecutorId);
        Task<bool> eliminarUsuario(int id, int usuarioEjecutorId);
        Task<Usuario> obtenerUsuario(int id);
        Task<List<Usuario>> obtenerUsuarios();
    }
}
