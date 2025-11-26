

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;

namespace Proyecto.BW.CU
{
    public class GestionClienteBW : IClienteBW
    {
        private readonly IClienteDA clienteDA;
        private readonly ICuentaDA cuentaDA;

        public GestionClienteBW(IClienteDA clienteDA, ICuentaDA cuentaDA)
        {
            this.clienteDA = clienteDA;
            this.cuentaDA = cuentaDA;
        }

        public async Task<bool> registrarCliente(Cliente cliente)
        {
            ReglasDeCliente.Validar(cliente);

            var existeCorreo = await clienteDA.obtenerPorCorreo(cliente.Correo);
            if (existeCorreo != null)
                throw new Exception("Ya existe un cliente con ese correo");

            var existeIdent = await clienteDA.obtenerPorIdentificacion(cliente.Identificacion);
            if (existeIdent != null)
                throw new Exception("Ya existe un cliente con esa identificacion");

            //Un usuario solo puede estar asociado a un cliente
            var existeUsuario = await clienteDA.obtenerPorUsuarioId(cliente.UsuarioId);
            if (existeUsuario != null)
                throw new Exception("Este UsuarioId ya está asociado a otro cliente");

            return await clienteDA.registrarCliente(cliente);
        }

        public async Task<bool> actualizarCliente(Cliente cliente, int id)
        {
            ReglasDeCliente.ValidarId(id);
            ReglasDeCliente.Validar(cliente);

            var clienteCorreo = await clienteDA.obtenerPorCorreo(cliente.Correo);
            if (clienteCorreo != null && clienteCorreo.ClienteId != id)
                throw new Exception("El correo ya esta en uso por otro cliente");

            var clienteIdent = await clienteDA.obtenerPorIdentificacion(cliente.Identificacion);
            if (clienteIdent != null && clienteIdent.ClienteId != id)
                throw new Exception("La identificacion ya esta en uso por otro cliente");

            var clienteUsuario = await clienteDA.obtenerPorUsuarioId(cliente.UsuarioId);
            if (clienteUsuario != null && clienteUsuario.ClienteId != id)
                throw new Exception("Este UsuarioId ya está asociado a otro cliente");

            return await clienteDA.actualizarCliente(cliente, id);
        }

        public async Task<bool> eliminarCliente(int id)
        {
            ReglasDeCliente.ValidarId(id);

            //Se valida que el cliente no tenga cuentas activas o bloqueadas
            var cuentas = await cuentaDA.obtenerCuentasPorCliente(id);

            if (cuentas.Any(c =>
                    c.EstadoCuenta == EstadoCuenta.Activa ||
                    c.EstadoCuenta == EstadoCuenta.Bloqueada))
            {
                throw new Exception("No se puede eliminar el cliente porque tiene cuentas activas o bloqueadas");
            }

            return await clienteDA.eliminarCliente(id);
        }

        public Task<Cliente> obtenerCliente(int id)
        {
            ReglasDeCliente.ValidarId(id);
            return clienteDA.obtenerCliente(id);
        }

        public Task<List<Cliente>> obtenerClientes()
        {
            return clienteDA.obtenerClientes();
        }

        public Task<Cliente?> obtenerPorCorreo(string correo)
        {
            return clienteDA.obtenerPorCorreo(correo);
        }

        public Task<Cliente?> obtenerPorIdentificacion(string identificacion)
        {
            return clienteDA.obtenerPorIdentificacion(identificacion);
        }

        public Task<Cliente?> obtenerPorUsuarioId(int usuarioId)
        {
            return clienteDA.obtenerPorUsuarioId(usuarioId);
        }
    }
}
