using Proyecto.BC.Modelos;


namespace Proyecto.BW.Interfaces.DA
{
    public interface IUsuarioDA
    {
        Task<bool> registrarUsuario(Usuario usuario);
        Task<bool> actualizarUsuario(Usuario usuario, int id);
        Task<bool> eliminarUsuario(int id);
        Task<Usuario> obtenerUsuario(int id);
        Task<List<Usuario>> obtenerUsuarios();

        // Metodos extra solo para validaciones internas
        Task<Usuario?> obtenerPorCorreo(string correo);
    }
}
