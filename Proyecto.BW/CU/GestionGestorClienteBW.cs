

using Proyecto.BC.Modelos;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;

namespace Proyecto.BW.CU
{
    public class GestionGestorClienteBW : IGestorClienteBW
    {
        private readonly IGestorClienteDA gestorClienteDA;
        private readonly IUsuarioDA usuarioDA;
        private readonly IClienteDA clienteDA;

        public GestionGestorClienteBW(IGestorClienteDA gestorClienteDA, IUsuarioDA usuarioDA, IClienteDA clienteDA)
        {
            this.gestorClienteDA = gestorClienteDA;
            this.usuarioDA = usuarioDA;
            this.clienteDA = clienteDA;
        }

        public async Task<bool> asignarGestorACliente(GestorCliente relacion)
        {
            // Validar que la relacion tenga datos basicos correctos
            ReglasDeGestorCliente.Validar(relacion);

            // Validar que el gestor exista
            var gestor = await usuarioDA.obtenerUsuario(relacion.GestorId);
            if (gestor == null)
                throw new Exception("El gestor indicado no existe");

            // Validar que el cliente exista
            var cliente = await clienteDA.obtenerCliente(relacion.ClienteId);
            if (cliente == null)
                throw new Exception("El cliente indicado no existe");

            // Opcional: Validar que esa relacion no exista ya
            var relacionesCliente = await gestorClienteDA.obtenerGestoresPorCliente(relacion.ClienteId);
            bool yaAsignado = relacionesCliente.Any(r => r.GestorId == relacion.GestorId);

            if (yaAsignado)
                throw new Exception("Este gestor ya esta asignado a este cliente");

            // Si todo esta bien -> asignar
            return await gestorClienteDA.asignarGestorACliente(relacion);
        }

        public async Task<bool> eliminarAsignacion(int gestorId, int clienteId)
        {
            if (gestorId <= 0)
                throw new Exception("El id del gestor debe ser mayor a cero");

            if (clienteId <= 0)
                throw new Exception("El id del cliente debe ser mayor a cero");

            return await gestorClienteDA.eliminarAsignacion(gestorId, clienteId);
        }

        public Task<List<GestorCliente>> obtenerClientesPorGestor(int gestorId)
        {
            if (gestorId <= 0)
                throw new Exception("El id del gestor debe ser mayor a cero");
            return gestorClienteDA.obtenerClientesPorGestor(gestorId);
        }

        public Task<List<GestorCliente>> obtenerGestoresPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return gestorClienteDA.obtenerGestoresPorCliente(clienteId);
        }
    }
}
