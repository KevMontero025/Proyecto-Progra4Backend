using Proyecto.BC.Modelos;
using System.Text;

namespace Proyecto.API.Helpers
{
    public class ComprobantePdfBuilder
    {
        // Simulación
        public byte[] GenerarPdfBasico(Comprobante c)
        {
            var contenido = $@"
                COMPROBANTE DE OPERACION

                Referencia: {c.NumeroReferencia}
                Tipo: {c.Tipo}
                Fecha: {c.Fecha}

                Cliente ID: {c.ClienteId}
                Cuenta ID: {c.CuentaId}
                Operacion ID: {c.OperacionId}
                ";

            return Encoding.UTF8.GetBytes(contenido);
        }
    }
}