using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Config;

namespace Proyecto.DA.Acciones
{
    public class GestionParametroSistemaDA : IParametroSistemaDA
    {
        private readonly BancoContext bancoContext;

        public GestionParametroSistemaDA(BancoContext bancoContext)
        {
            this.bancoContext = bancoContext;
        }

        public async Task<bool> registrarParametro(ParametroSistema parametro)
        {
            try
            {
                bancoContext.ParametroSistema.Add(parametro);
                await bancoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el parametro de sistema: " + ex.Message);
            }
        }

        public async Task<bool> actualizarParametro(ParametroSistema parametro, int id)
        {
            var existente = await bancoContext.ParametroSistema.FindAsync(id);
            if (existente == null)
                return false;

            existente.Clave = parametro.Clave;
            existente.Valor = parametro.Valor;
            existente.Descripcion = parametro.Descripcion;

            await bancoContext.SaveChangesAsync();
            return true;
        }

        public Task<ParametroSistema> obtenerParametroPorClave(string clave)
        {
            return bancoContext.ParametroSistema
                .FirstAsync(p => p.Clave == clave);
        }

        public Task<List<ParametroSistema>> obtenerParametros()
        {
            return bancoContext.ParametroSistema.ToListAsync();
        }
    }
}
