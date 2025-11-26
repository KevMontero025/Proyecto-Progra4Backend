

using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.Modelos
{
    // Representa una transferencia bancaria, normal o programada
    // Puede ser interna, cuenta a cuenta o hacia un beneficiario tercero
    // Por eso algunos campos son obligatorios y otros pueden venir vacios segun el tipo de transferencia
    public class Transferencia
    {
        public int TransferenciaId { get; set; }
        public int ClienteId { get; set; }
        public int CuentaOrigenId { get; set; }
        public int? CuentaDestinoId { get; set; }
        //Es nullable porque solo se usa cuando el destino es una cuenta del mismo banco
        // Si la transferencia es a un tercero, este campo queda null
        public int? TerceroBeneficiarioId { get; set; } // Es nullable porque solo se usa cuando el destino no es una cuenta interna
        // Si la transferencia es entre cuentas internas, esto queda null
        public decimal Monto { get; set; }
        public Moneda Moneda { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public bool EsProgramada { get; set; }// Si se hara despues
        public EstadoTransferencia Estado { get; set; }
        public decimal SaldoAntes { get; set; }
        public decimal SaldoDespues { get; set; }
        public decimal Comision { get; set; }// Comision cobrada
        public string IdempotencyKey { get; set; } // Sirve para evitar transferencias duplicadas si el usuario vuelve a enviar la solicitud
        public bool NecesitaAprobacion { get; set; } // Si excede monto limite
        public int? AprobadaPorUsuarioId { get; set; }
        //Si no necesita aprobacion, queda null, si si necesita, se guarda el UsuarioId del aprobador
    }
}
