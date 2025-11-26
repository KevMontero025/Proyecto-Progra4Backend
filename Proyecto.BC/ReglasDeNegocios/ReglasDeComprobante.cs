

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar un comprobante generado por una operacion.
    public static class ReglasDeComprobante
    {
        public static void Validar(Comprobante c)
        {
            if (c == null)
                throw new Exception("El comprobante no puede ser null");

            if (string.IsNullOrWhiteSpace(c.NumeroReferencia))
                throw new Exception("El numero de referencia del comprobante es obligatorio");

            if (string.IsNullOrWhiteSpace(c.Tipo))
                throw new Exception("El tipo de comprobante es obligatorio");

            if (c.OperacionId <= 0)
                throw new Exception("El comprobante debe estar asociado a una operacion valida");

            if (c.ClienteId <= 0)
                throw new Exception("El comprobante debe pertenecer a un cliente valido");

            if (c.CuentaId <= 0)
                throw new Exception("El comprobante debe estar ligado a una cuenta valida");

            if (c.Fecha == default(DateTime))
                throw new Exception("La fecha del comprobante no es valida");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de comprobante debe ser mayor a cero");
        }
    }
}
