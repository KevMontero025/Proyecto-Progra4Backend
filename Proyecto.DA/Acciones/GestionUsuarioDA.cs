using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.DA.Acciones
{
    public class GestionUsuarioDA : IUsuarioDA
    {
        private readonly BancoContext bancoContext;

        public GestionUsuarioDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> actualizarUsuario(Usuario usuario, int id)
        {
            var usuarioExistente = await bancoContext.Usuario.FindAsync(id);
            if (usuarioExistente == null)
                return false;

            usuarioExistente.Email = usuario.Email;
            usuarioExistente.PasswordHash = usuario.PasswordHash;
            usuarioExistente.Rol = usuario.Rol;
            usuarioExistente.IntentosFallidosLogin = usuario.IntentosFallidosLogin;
            usuarioExistente.IsLockedOut = usuario.IsLockedOut;
            usuarioExistente.LockoutEnd = usuario.LockoutEnd;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> eliminarUsuario(int id)
        {
            var usuario = await bancoContext.Usuario.FindAsync(id);
            if (usuario == null)
                return false;

            bancoContext.Usuario.Remove(usuario);
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<Usuario?> obtenerPorCorreo(string correo)
        {
            return bancoContext.Usuario
                .FirstOrDefaultAsync(u => u.Email == correo);
        }

        public Task<Usuario> obtenerUsuario(int id)
        {
            return bancoContext.Usuario
                .FirstAsync(u => u.UsuarioId == id);
        }

        public Task<List<Usuario>> obtenerUsuarios()
        {
            return bancoContext.Usuario.ToListAsync();
        }

        public async Task<bool> registrarUsuario(Usuario usuario)
        {
            try
            {
                bancoContext.Usuario.Add(usuario);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el usuario: " + ex.Message);
            }
        }
    }
}