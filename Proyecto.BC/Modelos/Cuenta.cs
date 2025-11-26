

using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.Modelos
{
    // Representa una cuenta bancaria de un cliente
    public class Cuenta
    {
        public int CuentaId { get; set; }
        public string NumeroCuenta { get; set; }
        public TipoCuenta TipoCuenta { get; set; }
        public Moneda Moneda { get; set; }
        public decimal Saldo { get; set; }
        public EstadoCuenta EstadoCuenta { get; set; }
        public int ClienteId { get; set; }
    }
}
