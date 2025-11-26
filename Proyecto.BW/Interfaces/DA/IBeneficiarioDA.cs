

using Proyecto.BC.Modelos;

namespace Proyecto.BW.Interfaces.DA
{
    public interface IBeneficiarioDA
    {
        Task<bool> registrarBeneficiario(TerceroBeneficiario beneficiario);
        Task<bool> actualizarBeneficiario(TerceroBeneficiario beneficiario, int id);
        Task<bool> eliminarBeneficiario(int id);
        Task<TerceroBeneficiario> obtenerBeneficiario(int id);
        Task<List<TerceroBeneficiario>> obtenerBeneficiariosPorCliente(int clienteId);
    }
}
