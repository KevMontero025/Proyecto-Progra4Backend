using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionTransaccionCuentaDA : ITransaccionCuentaDA
    {
        private readonly BancoContext bancoContext;

        public GestionTransaccionCuentaDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public Task<TransaccionCuenta> obtenerTransaccion(int id)
        {
            return bancoContext.TransaccionCuenta
                .FirstAsync(t => t.TransaccionCuentaId == id);
        }

        public Task<List<TransaccionCuenta>> obtenerTransaccionesPorCliente(int clienteId)
        {
            return bancoContext.TransaccionCuenta
                .Where(t => bancoContext.Cuenta
                    .Any(c => c.CuentaId == t.CuentaId && c.ClienteId == clienteId))
                .ToListAsync();
        }

        public Task<List<TransaccionCuenta>> obtenerTransaccionesPorCuenta(int cuentaId)
        {
            return bancoContext.TransaccionCuenta
                .Where(t => t.CuentaId == cuentaId)
                .ToListAsync();
        }

        public async Task<bool> registrarTransaccion(TransaccionCuenta transaccion)
        {
            try
            {
                bancoContext.TransaccionCuenta.Add(transaccion);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la transaccion: " + ex.Message);
            }
        }
    }
}