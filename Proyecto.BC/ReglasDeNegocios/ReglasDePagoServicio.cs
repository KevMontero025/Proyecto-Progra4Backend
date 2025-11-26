

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar un pago de servicio.
    public static class ReglasDePagoServicio
    {
        public static void Validar(PagoServicio pago)
        {
            if (pago == null)
                throw new Exception("El pago de servicio no puede ser null");

            if (pago.ClienteId <= 0)
                throw new Exception("El pago debe pertenecer a un cliente valido");

            if (pago.ProveedorServicioId <= 0)
                throw new Exception("Debe indicarse el proveedor de servicio");

            if (string.IsNullOrWhiteSpace(pago.NumeroContrato))
                throw new Exception("El numero de contrato es obligatorio");

            if (pago.CuentaOrigenId <= 0)
                throw new Exception("Debe indicarse la cuenta desde donde se pagara el servicio");

            if (pago.Monto <= 0)
                throw new Exception("El monto del pago debe ser mayor a cero");

            if (pago.FechaCreacion == default(DateTime))
                throw new Exception("La fecha de creacion del pago no es valida");

            if (pago.FechaEjecucion < pago.FechaCreacion)
                throw new Exception("La fecha de ejecucion del pago no puede ser menor a la fecha de creacion");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de pago de servicio debe ser mayor a cero");
        }
    }
}
