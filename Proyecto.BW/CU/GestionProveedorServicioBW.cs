
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System.Text.Json;

namespace Proyecto.BW.CU
{
    public class GestionProveedorServicioBW : IProveedorServicioBW
    {
        private readonly IProveedorServicioDA proveedorDA;
        private readonly IUsuarioDA usuarioDA;
        private readonly IAuditoriaBW auditoriaBW;

        public GestionProveedorServicioBW(
            IProveedorServicioDA proveedorDA,
            IUsuarioDA usuarioDA,
            IAuditoriaBW auditoriaBW)
        {
            this.proveedorDA = proveedorDA;
            this.usuarioDA = usuarioDA;
            this.auditoriaBW = auditoriaBW;
        }

        // Solo ADMIN puede registrar proveedor
        public async Task<bool> registrarProveedor(ProveedorServicio proveedor, int usuarioAccionId)
        {
            ReglasDeProveedorServicio.Validar(proveedor);

            // Validar que el usuario que ejecuta sea Admin
            var usuario = await usuarioDA.obtenerUsuario(usuarioAccionId);
            if (usuario.Rol != RolUsuario.Administrador)
                throw new Exception("Solo administradores pueden registrar proveedores de servicios.");

            var resultado = await proveedorDA.registrarProveedor(proveedor);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CreacionProveedorServicio,
                    Entidad = "ProveedorServicio",
                    EntidadId = proveedor.ProveedorServicioId.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(proveedor)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // Solo ADMIN puede actualizar proveedor
        public async Task<bool> actualizarProveedor(ProveedorServicio proveedor, int id, int usuarioAccionId)
        {
            ReglasDeProveedorServicio.ValidarId(id);
            ReglasDeProveedorServicio.Validar(proveedor);

            var usuario = await usuarioDA.obtenerUsuario(usuarioAccionId);
            if (usuario.Rol != RolUsuario.Administrador)
                throw new Exception("Solo administradores pueden actualizar proveedores de servicios.");

            // cargar estado anterior para auditoría
            var proveedorAnterior = await proveedorDA.obtenerProveedor(id);

            var resultado = await proveedorDA.actualizarProveedor(proveedor, id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.ActualizacionProveedorServicio,
                    Entidad = "ProveedorServicio",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(proveedorAnterior),
                    ValoresNuevos = JsonSerializer.Serialize(proveedor)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // Solo admin puede eliminar proveedor
        public async Task<bool> eliminarProveedor(int id, int usuarioAccionId)
        {
            ReglasDeProveedorServicio.ValidarId(id);

            var usuario = await usuarioDA.obtenerUsuario(usuarioAccionId);
            if (usuario.Rol != RolUsuario.Administrador)
                throw new Exception("Solo administradores pueden eliminar proveedores de servicios.");

            // Guardar datos antes de borrar para auditoría
            var proveedorAnterior = await proveedorDA.obtenerProveedor(id);

            var resultado = await proveedorDA.eliminarProveedor(id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.EliminacionProveedorServicio,
                    Entidad = "ProveedorServicio",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(proveedorAnterior),
                    ValoresNuevos = null
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public Task<ProveedorServicio> obtenerProveedor(int id)
        {
            ReglasDeProveedorServicio.ValidarId(id);
            return proveedorDA.obtenerProveedor(id);
        }

        public Task<List<ProveedorServicio>> obtenerProveedores()
        {
            return proveedorDA.obtenerProveedores();
        }
    }
}
