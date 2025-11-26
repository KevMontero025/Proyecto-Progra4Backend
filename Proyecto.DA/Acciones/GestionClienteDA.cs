using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.DA.Acciones
{
    public class GestionClienteDA : IClienteDA
    {
        private readonly BancoContext bancoContext;

        public GestionClienteDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> actualizarCliente(Cliente cliente, int id)
        {
            var clienteExistente = await bancoContext.Cliente.FindAsync(id);
            if (clienteExistente == null)
                return false; // Cliente no encontrado

            clienteExistente.NombreCompleto = cliente.NombreCompleto;
            clienteExistente.Identificacion = cliente.Identificacion;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Correo = cliente.Correo;
            clienteExistente.UsuarioId = cliente.UsuarioId;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> eliminarCliente(int id)
        {
            var cliente = await bancoContext.Cliente.FindAsync(id);
            if (cliente == null)
                return false;

            bancoContext.Cliente.Remove(cliente);
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<Cliente> obtenerCliente(int id)
        {
            return bancoContext.Cliente.FirstAsync(c => c.ClienteId == id);
        }

        public Task<List<Cliente>> obtenerClientes()
        {
            return bancoContext.Cliente.ToListAsync();
        }

        public Task<Cliente?> obtenerPorCorreo(string correo)
        {
            return bancoContext.Cliente.FirstOrDefaultAsync(c => c.Correo == correo);
        }

        public Task<Cliente?> obtenerPorIdentificacion(string identificacion)
        {
            return bancoContext.Cliente.FirstOrDefaultAsync(c => c.Identificacion == identificacion);
        }

        public async Task<bool> registrarCliente(Cliente cliente)
        {
            try
            {
                bancoContext.Cliente.Add(cliente);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el Cliente: " + ex.Message);
            }
        }
    }
}
