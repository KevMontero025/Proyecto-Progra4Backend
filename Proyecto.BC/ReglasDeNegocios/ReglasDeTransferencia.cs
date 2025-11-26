

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar una transferencia antes de intentar guardarla o ejecutarla.
    public static class ReglasDeTransferencia
    {
        public static void Validar(Transferencia t)
        {
            if (t == null)
                throw new Exception("La transferencia no puede ser null");

            if (t.ClienteId <= 0)
                throw new Exception("La transferencia debe estar asociada a un cliente valido");

            if (t.CuentaOrigenId <= 0)
                throw new Exception("Debe indicarse la cuenta de origen");

            ValidarDestino(t);

            if (t.Monto <= 0)
                throw new Exception("El monto de la transferencia debe ser mayor a cero");

            if (t.FechaCreacion == default(DateTime))
                throw new Exception("La fecha de creacion de la transferencia no es valida");

            if (t.FechaEjecucion < t.FechaCreacion)
                throw new Exception("La fecha de ejecucion no puede ser anterior a la fecha de creacion");
        }

        // Reglas para saber si el destino esta bien definido.
        private static void ValidarDestino(Transferencia t)
        {
            bool destinoInterno = t.CuentaDestinoId.HasValue && !t.TerceroBeneficiarioId.HasValue;
            bool destinoTercero = !t.CuentaDestinoId.HasValue && t.TerceroBeneficiarioId.HasValue;

            if (!destinoInterno && !destinoTercero)
            {
                throw new Exception(
                    "La transferencia debe tener un destino valido: " +
                    "o una cuenta interna destino o un beneficiario tercero, pero no ambos"
                );
            }
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de transferencia debe ser mayor a cero");
        }
    }
}
