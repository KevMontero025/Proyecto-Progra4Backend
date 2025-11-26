using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;
using Microsoft.EntityFrameworkCore;


namespace Proyecto.DA.Acciones
{
    public class GestionBeneficiarioDA : IBeneficiarioDA
    {
        private readonly BancoContext bancoContext;

        public GestionBeneficiarioDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> actualizarBeneficiario(TerceroBeneficiario beneficiario, int id)
        {
            var beneficiarioExistente = await bancoContext.TerceroBeneficiario.FindAsync(id);
            if (beneficiarioExistente == null)
                return false;

            beneficiarioExistente.ClienteId = beneficiario.ClienteId;
            beneficiarioExistente.Alias = beneficiario.Alias;
            beneficiarioExistente.Banco = beneficiario.Banco;
            beneficiarioExistente.NumeroCuenta = beneficiario.NumeroCuenta;
            beneficiarioExistente.Moneda = beneficiario.Moneda;
            beneficiarioExistente.Pais = beneficiario.Pais;
            beneficiarioExistente.Estado = beneficiario.Estado;
            beneficiarioExistente.Confirmado = beneficiario.Confirmado;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> eliminarBeneficiario(int id)
        {
            var beneficiario = await bancoContext.TerceroBeneficiario.FindAsync(id);
            if (beneficiario == null)
                return false;

            bancoContext.TerceroBeneficiario.Remove(beneficiario);
            await bancoContext.SaveChangesAsync();
            return true;
        }

        public async Task<TerceroBeneficiario> obtenerBeneficiario(int id)
        {
            return await bancoContext.TerceroBeneficiario.FirstAsync(b => b.TerceroBeneficiarioId == id);
        }

        public Task<List<TerceroBeneficiario>> obtenerBeneficiariosPorCliente(int clienteId)
        {
            return bancoContext.TerceroBeneficiario
                .Where(b => b.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<bool> registrarBeneficiario(TerceroBeneficiario beneficiario)
        {
            try
            {
                bancoContext.TerceroBeneficiario.Add(beneficiario);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el beneficiario: " + ex.Message);
            }
        }
    }
}
