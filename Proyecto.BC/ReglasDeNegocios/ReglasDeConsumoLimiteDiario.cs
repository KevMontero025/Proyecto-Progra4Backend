using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar el registro de consumo diario de transferencias de un cliente.
    public static class ReglasDeConsumoLimiteDiario
    {
        public static void Validar(ConsumoLimiteDiario c)
        {
            if (c == null)
                throw new Exception("El consumo diario no puede ser null");

            if (c.ClienteId <= 0)
                throw new Exception("El consumo diario debe pertenecer a un cliente valido");

            if (c.Fecha == default(DateTime))
                throw new Exception("La fecha del consumo diario no es valida");

            if (c.MontoTotalTransferido < 0)
                throw new Exception("El monto total transferido del dia no puede ser negativo");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de consumo diario debe ser mayor a cero");
        }
    }
}
