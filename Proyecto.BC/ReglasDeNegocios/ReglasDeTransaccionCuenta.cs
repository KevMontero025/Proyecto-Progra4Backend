
using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar un movimiento en la cuenta (cargo o abono).
    public static class ReglasDeTransaccionCuenta
    {
        public static void Validar(TransaccionCuenta tx)
        {
            if (tx == null)
                throw new Exception("La transaccion de cuenta no puede ser null");

            if (tx.CuentaId <= 0)
                throw new Exception("La transaccion debe pertenecer a una cuenta valida");

            if (tx.Fecha == default(DateTime))
                throw new Exception("La fecha de la transaccion no es valida");

            if (tx.Monto == 0)
                throw new Exception("El monto de la transaccion no puede ser cero");

            if (string.IsNullOrWhiteSpace(tx.EstadoOperacion))
                throw new Exception("El estado de la transaccion es obligatorio");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de transaccion debe ser mayor a cero");
        }
    }
}
