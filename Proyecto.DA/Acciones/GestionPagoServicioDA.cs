using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionPagoServicioDA : IPagoServicioDA
    {
        private readonly BancoContext bancoContext;

        public GestionPagoServicioDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> realizarPago(PagoServicio pago)
        {
            try
            {
                bancoContext.PagoServicio.Add(pago);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago de servicio: " + ex.Message);
            }
        }

        public async Task<bool> cancelarPago(int id)
        {
            var pago = await bancoContext.PagoServicio.FindAsync(id);
            if (pago == null)
                return false;

            // Aqui solo marcamos el estado como cancelado.
            // Ajusta el valor segun tu enum EstadoPagoServicio.
            pago.Estado = EstadoPagoServicio.Cancelado;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<PagoServicio> obtenerPago(int id)
        {
            return bancoContext.PagoServicio
                .FirstAsync(p => p.PagoServicioId == id);
        }

        public Task<List<PagoServicio>> obtenerPagosPorCliente(int clienteId)
        {
            return bancoContext.PagoServicio
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.FechaEjecucion)
                .ToListAsync();
        }
    }
}
