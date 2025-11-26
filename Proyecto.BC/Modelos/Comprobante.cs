

namespace Proyecto.BC.Modelos
{
    public class Comprobante
    {
        public int ComprobanteId { get; set; }
        public string NumeroReferencia { get; set; } // Codigo único del comprobante
        public string Tipo { get; set; } // Transferencia / PagoServicio
        public int OperacionId { get; set; }// Id de la operacion asociada
        public int ClienteId { get; set; }
        public int CuentaId { get; set; }
        public DateTime Fecha { get; set; }
        public string RutaArchivo { get; set; }// Dónde se guarda el PDF
        public string DatosJson { get; set; }// Datos para regenerar comprobante
    }
}
