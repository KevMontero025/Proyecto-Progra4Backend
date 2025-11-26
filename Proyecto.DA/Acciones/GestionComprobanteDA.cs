using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionComprobanteDA : IComprobanteDA
    {
        private readonly BancoContext bancoContext;

        public GestionComprobanteDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> generarComprobante(Comprobante comprobante)
        {
            try
            {
                await bancoContext.Comprobante.AddAsync(comprobante);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el comprobante: " + ex.Message);
            }
        }

        public Task<Comprobante> obtenerComprobante(int id)
        {
            return bancoContext.Comprobante.FirstAsync(c => c.ComprobanteId == id);
        }

        public Task<List<Comprobante>> obtenerComprobantesPorCliente(int clienteId)
        {
            return bancoContext.Comprobante
                .Where(c => c.ClienteId == clienteId)
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();
        }
    }
}
