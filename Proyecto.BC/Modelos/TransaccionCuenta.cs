using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.Modelos
{
    // Movimiento dentro de una cuenta cargos y abonos
    public class TransaccionCuenta
    {
        public int TransaccionCuentaId { get; set; }
        public int CuentaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }
        public TipoOperacionCuenta TipoOperacion { get; set; }
        public EstadoOperacionCuenta EstadoOperacion { get; set; }
        public int? TransferenciaId { get; set; }
        public int? PagoServicioId { get; set; }
    }
}
