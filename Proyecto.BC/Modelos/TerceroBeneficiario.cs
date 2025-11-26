

using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.Modelos
{
    // Cuenta de un tercero agregado por un cliente para hacerle transferencias
    public class TerceroBeneficiario
    {
        public int TerceroBeneficiarioId { get; set; }
        public int ClienteId { get; set; }
        public string Alias { get; set; }// Nombre asignado por el cliente
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public Moneda Moneda { get; set; }
        public string Pais { get; set; }
        public EstadoBeneficiario Estado { get; set; }
        public bool Confirmado { get; set; }
    }
}
