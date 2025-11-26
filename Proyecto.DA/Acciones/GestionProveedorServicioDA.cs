using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionProveedorServicioDA : IProveedorServicioDA
    {
        private readonly BancoContext bancoContext;

        public GestionProveedorServicioDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> actualizarProveedor(ProveedorServicio proveedor, int id)
        {
            var provExistente = await bancoContext.ProveedorServicio.FindAsync(id);
            if (provExistente == null)
                return false;

            provExistente.Nombre = proveedor.Nombre;
            provExistente.LongitudMinContrato = proveedor.LongitudMinContrato;
            provExistente.LongitudMaxContrato = proveedor.LongitudMaxContrato;
            provExistente.ReglasAdicionales = proveedor.ReglasAdicionales;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> eliminarProveedor(int id)
        {
            var prov = await bancoContext.ProveedorServicio.FindAsync(id);
            if (prov == null)
                return false;

            bancoContext.ProveedorServicio.Remove(prov);
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<ProveedorServicio> obtenerProveedor(int id)
        {
            return bancoContext.ProveedorServicio
                .FirstAsync(p => p.ProveedorServicioId == id);
        }

        public Task<List<ProveedorServicio>> obtenerProveedores()
        {
            return bancoContext.ProveedorServicio.ToListAsync();
        }

        public async Task<bool> registrarProveedor(ProveedorServicio proveedor)
        {
            try
            {
                bancoContext.ProveedorServicio.Add(proveedor);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el proveedor: " + ex.Message);
            }
        }
    }
}