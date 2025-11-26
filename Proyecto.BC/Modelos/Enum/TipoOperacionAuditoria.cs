

namespace Proyecto.BC.Modelos.Enum
{
    public enum TipoOperacionAuditoria // Tipo de accion importante que se guarda en auditoria
    {
        CreacionUsuario,
        ActualizacionUsuario,
        EliminacionUsuario,
        AperturaCuenta,
        BloqueoCuenta,
        CierreCuenta,
        PagoServicio,
        CancelacionPago,
        CreacionParametroSistema,
        ActualizacionParametroSistema,
        CreacionTransferencia,
        CancelacionTransferencia,
        CreacionBeneficiario,
        ActualizacionBeneficiario,
        EliminacionBeneficiario,
        AprobacionTransferencia,
        RechazoTransferencia,
        CreacionProveedorServicio,
        ActualizacionProveedorServicio,
        EliminacionProveedorServicio

    }
}
