using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionCuentaDA : ICuentaDA
    {
        private readonly BancoContext bancoContext;

        public GestionCuentaDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> crearCuenta(Cuenta cuenta)
        {
            try
            {
                bancoContext.Cuenta.Add(cuenta);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la cuenta: " + ex.Message);
            }
        }

        public async Task<bool> actualizarCuenta(Cuenta cuenta, int id)
        {
            var cuentaExistente = await bancoContext.Cuenta.FindAsync(id);
            if (cuentaExistente == null)
                return false;

            cuentaExistente.NumeroCuenta = cuenta.NumeroCuenta;
            cuentaExistente.TipoCuenta = cuenta.TipoCuenta;
            cuentaExistente.Moneda = cuenta.Moneda;
            cuentaExistente.Saldo = cuenta.Saldo;
            cuentaExistente.EstadoCuenta = cuenta.EstadoCuenta;
            cuentaExistente.ClienteId = cuenta.ClienteId;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> bloquearCuenta(int id)
        {
            var cuenta = await bancoContext.Cuenta.FindAsync(id);
            if (cuenta == null)
                return false;

            // Marcamos la cuenta como bloqueada
            cuenta.EstadoCuenta = EstadoCuenta.Bloqueada;
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> cerrarCuenta(int id)
        {
            var cuenta = await bancoContext.Cuenta.FindAsync(id);
            if (cuenta == null)
                return false;

            // Marcamos la cuenta como cerrada
            cuenta.EstadoCuenta = EstadoCuenta.Cerrada;
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<Cuenta> obtenerCuenta(int id)
        {
            return bancoContext.Cuenta
                .FirstAsync(c => c.CuentaId == id);
        }

        public Task<Cuenta?> obtenerCuentaPorNumero(string numeroCuenta)
        {
            return bancoContext.Cuenta
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }

        public Task<List<Cuenta>> obtenerCuentasPorCliente(int clienteId)
        {
            return bancoContext.Cuenta
                .Where(c => c.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<List<Cuenta>> filtrarCuentas(
            int? clienteId,
            TipoCuenta? tipo,
            Moneda? moneda,
            EstadoCuenta? estado)
        {
            var query = bancoContext.Cuenta.AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(c => c.ClienteId == clienteId.Value);

            if (tipo.HasValue)
                query = query.Where(c => c.TipoCuenta == tipo.Value);

            if (moneda.HasValue)
                query = query.Where(c => c.Moneda == moneda.Value);

            if (estado.HasValue)
                query = query.Where(c => c.EstadoCuenta == estado.Value);

            return await query.ToListAsync();
        }
    }
}