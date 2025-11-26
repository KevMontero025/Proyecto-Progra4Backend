
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BW.Interfaces.DA
{
    public interface ICuentaDA
    {
        Task<bool> crearCuenta(Cuenta cuenta);
        Task<bool> actualizarCuenta(Cuenta cuenta, int id);

        Task<bool> bloquearCuenta(int id);
        Task<bool> cerrarCuenta(int id);
        Task<Cuenta> obtenerCuenta(int id);
        Task<Cuenta?> obtenerCuentaPorNumero(string numeroCuenta);
        Task<List<Cuenta>> obtenerCuentasPorCliente(int clienteId);
        Task<List<Cuenta>> filtrarCuentas(int? clienteId,
                                          TipoCuenta? tipo,
                                          Moneda? moneda,
                                          EstadoCuenta? estado);
    }
}
