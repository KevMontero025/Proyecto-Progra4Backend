

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IParametroSistemaDA
    {
        Task<bool> registrarParametro(ParametroSistema parametro);
        Task<bool> actualizarParametro(ParametroSistema parametro, int id);
        Task<ParametroSistema> obtenerParametroPorClave(string clave);
        Task<List<ParametroSistema>> obtenerParametros();
    }
}
