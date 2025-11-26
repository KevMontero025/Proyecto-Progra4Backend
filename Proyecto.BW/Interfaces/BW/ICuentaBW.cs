

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BW.Interfaces.BW
{
    public interface ICuentaBW
    {
        Task<bool> crearCuenta(Cuenta cuenta, int usuarioEjecutorId);
        Task<bool> bloquearCuenta(int cuentaId, int usuarioEjecutorId);
        Task<bool> cerrarCuenta(int cuentaId, int usuarioEjecutorId);
        Task<Cuenta> obtenerCuenta(int id);
        Task<List<Cuenta>> obtenerCuentasPorCliente(int clienteId);
        Task<List<Cuenta>> filtrarCuentas(int usuarioId, int? clienteId = null,
                                          TipoCuenta? tipo = null,
                                          Moneda? moneda = null,
                                          EstadoCuenta? estado = null);
    }
}
