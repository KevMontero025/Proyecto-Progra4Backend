

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IParametroSistemaBW
    {
        Task<bool> registrarParametro(ParametroSistema parametro, int usuarioEjecutorId);
        Task<bool> actualizarParametro(ParametroSistema parametro, int id, int usuarioEjecutorId);
        Task<ParametroSistema> obtenerParametroPorClave(string clave);
        Task<List<ParametroSistema>> obtenerParametros();
    }
}
