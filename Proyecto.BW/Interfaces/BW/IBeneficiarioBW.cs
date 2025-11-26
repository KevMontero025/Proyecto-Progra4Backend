

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.BW
{
    public interface IBeneficiarioBW
    {
        Task<bool> registrarBeneficiario(TerceroBeneficiario beneficiario, int usuarioAccionId);
        Task<bool> actualizarBeneficiario(TerceroBeneficiario beneficiario, int id, int usuarioAccionId);
        Task<bool> eliminarBeneficiario(int id, int usuarioAccionId);
        Task<TerceroBeneficiario> obtenerBeneficiario(int id);
        Task<List<TerceroBeneficiario>> obtenerBeneficiariosPorCliente(int clienteId);
    }
}
