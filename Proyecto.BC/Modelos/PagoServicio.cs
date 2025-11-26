using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.Modelos
{
    // Pago de un servicio como luz, agua, teléfono, etc.
    public class PagoServicio
    {
        public int PagoServicioId { get; set; }
        public int ClienteId { get; set; }
        public int ProveedorServicioId { get; set; }
        public string NumeroContrato { get; set; } //Dato del proveedor
        public decimal Monto { get; set; }
        public Moneda Moneda { get; set; }
        public int CuentaOrigenId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public bool EsProgramado { get; set; }
        public EstadoPagoServicio Estado { get; set; }
        public string NumeroReferencia { get; set; } //Para comprobante
        public decimal Comision { get; set; }
    }
}
