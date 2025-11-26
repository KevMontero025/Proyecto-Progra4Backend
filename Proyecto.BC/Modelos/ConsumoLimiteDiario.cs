

namespace Proyecto.BC.Modelos
{
    // Acumulado de transferencias del cliente por dia
    public class ConsumoLimiteDiario
    {
        public int ConsumoLimiteDiarioId { get; set; }
        public int ClienteId { get; set; }
        public DateTime Fecha { get; set; } 
        public decimal MontoTotalTransferido { get; set; }
    }
}
