

namespace Proyecto.BC.Modelos.Enum
{
    public enum EstadoPagoServicio
    {// Resultado de un pago de servicio
        Programado, // Queda para otro dia
        Exitoso,
        Fallido,
        Ejecutado,
        Cancelado,
        Rechazado // El sistema no lo permitio
    }
}
