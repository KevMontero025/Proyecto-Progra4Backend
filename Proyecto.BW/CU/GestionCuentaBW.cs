

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System.Text.Json;

namespace Proyecto.BW.CU
{
    public class GestionCuentaBW : ICuentaBW
    {
        private readonly ICuentaDA cuentaDA;
        private readonly IUsuarioDA usuarioDA;
        private readonly IClienteDA clienteDA;
        private readonly ITransaccionCuentaDA transaccionDA;
        private readonly IAuditoriaBW auditoriaBW;

        public GestionCuentaBW(
            ICuentaDA cuentaDA,
            IUsuarioDA usuarioDA,
            IClienteDA clienteDA,
            ITransaccionCuentaDA transaccionDA,
            IAuditoriaBW auditoriaBW)
        {
            this.cuentaDA = cuentaDA;
            this.usuarioDA = usuarioDA;
            this.clienteDA = clienteDA;
            this.transaccionDA = transaccionDA;
            this.auditoriaBW = auditoriaBW;
        }

        // CREAR CUENTA
        // usuarioCreadorId = admin o gestor que abre la cuenta
        public async Task<bool> crearCuenta(Cuenta cuenta, int usuarioCreadorId)
        {
            ReglasDeCuenta.ValidarCreacion(cuenta);

            var usuario = await usuarioDA.obtenerUsuario(usuarioCreadorId);
            if (usuario.Rol != RolUsuario.Administrador &&
                usuario.Rol != RolUsuario.Gestor)
                throw new Exception("Solo administradores o gestores pueden crear cuentas.");

            var repetida = await cuentaDA.obtenerCuentaPorNumero(cuenta.NumeroCuenta);
            if (repetida != null)
                throw new Exception("El numero de cuenta ya existe.");

            // Validar que el cliente no tenga mas de 3 cuentas del mismo tipo y moneda
            var cuentasCliente = await cuentaDA.obtenerCuentasPorCliente(cuenta.ClienteId);

            int cantidad = cuentasCliente
                .Where(c => c.TipoCuenta == cuenta.TipoCuenta &&
                            c.Moneda == cuenta.Moneda)
                .Count();

            if (cantidad >= 3)
                throw new Exception("El cliente ya tiene 3 cuentas de este tipo y moneda.");

            cuenta.EstadoCuenta = EstadoCuenta.Activa;

            var resultado = await cuentaDA.crearCuenta(cuenta);

            if (resultado)
            {
                // Auditoria: apertura de cuenta
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioCreadorId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.AperturaCuenta,
                    Entidad = "Cuenta",
                    EntidadId = cuenta.CuentaId.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(cuenta)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // CONSULTA CON FILTROS DEPENDIENDO DEL ROL
        public async Task<List<Cuenta>> filtrarCuentas(
            int usuarioId,
            int? clienteId = null,
            TipoCuenta? tipo = null,
            Moneda? moneda = null,
            EstadoCuenta? estado = null)
        {
            var usuario = await usuarioDA.obtenerUsuario(usuarioId);

            // Si es cliente, solo puede ver sus cuentas
            if (usuario.Rol == RolUsuario.Cliente)
            {
                var cliente = await clienteDA.obtenerPorUsuarioId(usuario.UsuarioId);

                if (cliente == null)
                    throw new Exception("No se encontro un cliente asociado al usuario.");

                return await cuentaDA.obtenerCuentasPorCliente(cliente.ClienteId);
            }

            // Admin y Gestor pueden ver con filtros
            return await cuentaDA.filtrarCuentas(clienteId, tipo, moneda, estado);
        }

        public Task<Cuenta> obtenerCuenta(int id)
        {
            return cuentaDA.obtenerCuenta(id);
        }

        public Task<List<Cuenta>> obtenerCuentasPorCliente(int clienteId)
        {
            return cuentaDA.obtenerCuentasPorCliente(clienteId);
        }

        // BLOQUEO DE CUENTA
        // usuarioAdminId = administrador que hace el bloqueo
        public async Task<bool> bloquearCuenta(int cuentaId, int usuarioAdminId)
        {
            var usuario = await usuarioDA.obtenerUsuario(usuarioAdminId);

            if (usuario.Rol != RolUsuario.Administrador)
                throw new Exception("Solo administradores pueden bloquear cuentas.");

            // Cargamos el estado antes del cambio para auditoria
            var cuentaAntes = await cuentaDA.obtenerCuenta(cuentaId);
            ReglasDeCuenta.ValidarBloqueo(cuentaAntes);

            var resultado = await cuentaDA.bloquearCuenta(cuentaId);

            if (resultado)
            {
                // Volvemos a cargar la cuenta para tener el estado final
                var cuentaDespues = await cuentaDA.obtenerCuenta(cuentaId);

                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAdminId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.BloqueoCuenta,
                    Entidad = "Cuenta",
                    EntidadId = cuentaId.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(cuentaAntes),
                    ValoresNuevos = JsonSerializer.Serialize(cuentaDespues)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // CIERRE DE CUENTA
        // usuarioAdminId = administrador que cierra la cuenta
        public async Task<bool> cerrarCuenta(int cuentaId, int usuarioAdminId)
        {
            var usuario = await usuarioDA.obtenerUsuario(usuarioAdminId);

            if (usuario.Rol != RolUsuario.Administrador)
                throw new Exception("Solo administradores pueden cerrar cuentas.");

            var cuentaAntes = await cuentaDA.obtenerCuenta(cuentaId);

            ReglasDeCuenta.ValidarCierre(cuentaAntes);

            // Revisar que no existan movimientos pendientes
            var movimientos = await transaccionDA.obtenerTransaccionesPorCuenta(cuentaId);

            if (movimientos.Any(m =>
                    m.Descripcion != null &&
                    m.Descripcion.Contains("pendiente", StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("La cuenta tiene operaciones pendientes y no puede cerrarse.");
            }

            var resultado = await cuentaDA.cerrarCuenta(cuentaId);

            if (resultado)
            {
                var cuentaDespues = await cuentaDA.obtenerCuenta(cuentaId);

                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAdminId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CierreCuenta,
                    Entidad = "Cuenta",
                    EntidadId = cuentaId.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(cuentaAntes),
                    ValoresNuevos = JsonSerializer.Serialize(cuentaDespues)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }
    }
}