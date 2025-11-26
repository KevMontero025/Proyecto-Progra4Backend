

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System.Text.Json;

namespace Proyecto.BW.CU
{
    // Aqui se aplican las reglas de negocio y se registran las acciones en auditoria
    public class GestionUsuarioBW : IUsuarioBW
    {

        private readonly IUsuarioDA usuarioDA;
        private readonly IAuditoriaBW auditoriaBW;

        public GestionUsuarioBW(IUsuarioDA usuarioDA, IAuditoriaBW auditoriaBW)
        {
            this.usuarioDA = usuarioDA;
            this.auditoriaBW = auditoriaBW;
        }

        public async Task<bool> registrarUsuario(Usuario usuario, int usuarioEjecutorId)
        {
            // Validar datos basicos del usuario
            ReglasDeUsuario.Validar(usuario);

            // Validar email unico
            var usuarioConMismoCorreo = await usuarioDA.obtenerPorCorreo(usuario.Email);
            if (usuarioConMismoCorreo != null)
                throw new Exception("Ya existe un usuario con ese correo");

            var resultado = await usuarioDA.registrarUsuario(usuario);

            if (resultado)
            {
                // Registrar en auditoria la creacion del usuario
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioEjecutorId, // quien hizo la accion
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CreacionUsuario,
                    Entidad = "Usuario",
                    EntidadId = usuario.UsuarioId.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(usuario)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public async Task<bool> actualizarUsuario(Usuario usuario, int id, int usuarioEjecutorId)
        {
            ReglasDeUsuario.ValidarId(id);
            ReglasDeUsuario.Validar(usuario);

            // Obtener estado anterior para auditoria
            var usuarioAnterior = await usuarioDA.obtenerUsuario(id);
            if (usuarioAnterior == null)
                throw new Exception("El usuario que se quiere actualizar no existe");

            // Validar email unico ignorando al mismo usuario
            var usuarioCorreo = await usuarioDA.obtenerPorCorreo(usuario.Email);
            if (usuarioCorreo != null && usuarioCorreo.UsuarioId != id)
                throw new Exception("El correo ya esta en uso por otro usuario");

            var resultado = await usuarioDA.actualizarUsuario(usuario, id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioEjecutorId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.ActualizacionUsuario,
                    Entidad = "Usuario",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(usuarioAnterior),
                    ValoresNuevos = JsonSerializer.Serialize(usuario)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public async Task<bool> eliminarUsuario(int id, int usuarioEjecutorId)
        {
            ReglasDeUsuario.ValidarId(id);

            // Levantamos el usuario antes de eliminarlo para poder auditarlo
            Usuario? usuarioAnterior = null;
            try
            {
                usuarioAnterior = await usuarioDA.obtenerUsuario(id);
            }
            catch
            {
                // Si no existe, usuarioAnterior queda null y usamos solo el id
            }

            var resultado = await usuarioDA.eliminarUsuario(id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioEjecutorId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.EliminacionUsuario,
                    Entidad = "Usuario",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = usuarioAnterior != null
                        ? JsonSerializer.Serialize(usuarioAnterior): null,
                    ValoresNuevos = null // despues de eliminar ya no hay entidad
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public Task<Usuario> obtenerUsuario(int id)
        {
            ReglasDeUsuario.ValidarId(id);
            return usuarioDA.obtenerUsuario(id);
        }

        public Task<List<Usuario>> obtenerUsuarios()
        {
            return usuarioDA.obtenerUsuarios();
        }
    }
}
