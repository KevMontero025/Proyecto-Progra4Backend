using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionGestorClienteDA : IGestorClienteDA
    {
        private readonly BancoContext bancoContext;

        public GestionGestorClienteDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> asignarGestorACliente(GestorCliente relacion)
        {
            try
            {
                bancoContext.GestorCliente.Add(relacion);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar gestor a cliente: " + ex.Message);
            }
        }

        public async Task<bool> eliminarAsignacion(int gestorId, int clienteId)
        {
            var relacion = await bancoContext.GestorCliente
                .FirstOrDefaultAsync(gc => gc.GestorId == gestorId &&
                                           gc.ClienteId == clienteId);

            if (relacion == null)
                return false;

            bancoContext.GestorCliente.Remove(relacion);
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<List<GestorCliente>> obtenerClientesPorGestor(int gestorId)
        {
            return bancoContext.GestorCliente
                .Where(gc => gc.GestorId == gestorId)
                .ToListAsync();
        }

        public Task<List<GestorCliente>> obtenerGestoresPorCliente(int clienteId)
        {
            return bancoContext.GestorCliente
                .Where(gc => gc.ClienteId == clienteId)
                .ToListAsync();
        }
    }
}
