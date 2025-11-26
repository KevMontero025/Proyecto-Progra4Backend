
using System.Text.Json;
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;

namespace Proyecto.BW.CU
{
    public class GestionPagoServicioBW : IPagoServicioBW
    {
        private readonly IPagoServicioDA pagoServicioDA;
        private readonly ICuentaDA cuentaDA;
        private readonly IConsumoLimiteDiarioDA consumoDA;
        private readonly IParametroSistemaDA parametroDA;
        private readonly IProveedorServicioDA proveedorDA;
        private readonly IAuditoriaBW auditoriaBW;
        private readonly IComprobanteBW comprobanteBW;
        private readonly ITransaccionCuentaBW transaccionCuentaBW;

        public GestionPagoServicioBW(
            IPagoServicioDA pagoServicioDA,
            ICuentaDA cuentaDA,
            IConsumoLimiteDiarioDA consumoDA,
            IParametroSistemaDA parametroDA,
            IProveedorServicioDA proveedorDA,
            IAuditoriaBW auditoriaBW,
            IComprobanteBW comprobanteBW,
            ITransaccionCuentaBW transaccionCuentaBW)

        {
            this.pagoServicioDA = pagoServicioDA;
            this.cuentaDA = cuentaDA;
            this.consumoDA = consumoDA;
            this.parametroDA = parametroDA;
            this.proveedorDA = proveedorDA;
            this.auditoriaBW = auditoriaBW;
            this.comprobanteBW = comprobanteBW;
            this.transaccionCuentaBW = transaccionCuentaBW;
        }

        //Helper para referencia única
        private string GenerarReferenciaUnicaPago()
        {
            return $"PAG-{Guid.NewGuid():N}";
        }

        public async Task<bool> realizarPago(PagoServicio pago, int usuarioAccionId)
        {
            ReglasDePagoServicio.Validar(pago);

            var proveedor = await proveedorDA.obtenerProveedor(pago.ProveedorServicioId);
            if (proveedor == null)
                throw new Exception("El proveedor de servicio no existe");

            if (string.IsNullOrWhiteSpace(pago.NumeroContrato))
                throw new Exception("El numero de contrato es obligatorio");

            var largoContrato = pago.NumeroContrato.Trim().Length;
            if (largoContrato < proveedor.LongitudMinContrato || largoContrato > proveedor.LongitudMaxContrato)
                throw new Exception("El numero de contrato no cumple la longitud permitida");

            if (!pago.NumeroContrato.All(char.IsDigit))
                throw new Exception("El numero de contrato debe tener solo dígitos");

            // Cuenta origen
            var cuentaOrigen = await cuentaDA.obtenerCuenta(pago.CuentaOrigenId);
            if (cuentaOrigen == null)
                throw new Exception("La cuenta origen no existe");

            if (cuentaOrigen.EstadoCuenta != EstadoCuenta.Activa)
                throw new Exception("La cuenta origen debe estar activa");

            if (pago.Monto <= 0)
                throw new Exception("Monto inválido");

            if (cuentaOrigen.Saldo < pago.Monto)
                throw new Exception("Saldo insuficiente");

            // Límite diario
            var parametroLimite = await parametroDA.obtenerParametroPorClave("LIMITE_DIARIO_PAGOS");

            decimal limiteDiario = 0;
            if (parametroLimite != null && decimal.TryParse(parametroLimite.Valor, out var temp))
                limiteDiario = temp;

            var hoy = DateTime.Today;
            var consumoHoy = await consumoDA.obtenerConsumoPorFecha(pago.ClienteId, hoy);

            decimal montoConsumidoHoy = consumoHoy?.MontoTotalTransferido ?? 0;

            if (limiteDiario > 0 && montoConsumidoHoy + pago.Monto > limiteDiario)
                throw new Exception("Supera el límite diario");

            // Pagos programados
            if (pago.EsProgramado)
            {
                if (pago.FechaEjecucion.Date <= DateTime.Today)
                    throw new Exception("Fecha invalida para pago programado");

                if (pago.FechaEjecucion.Date > DateTime.Today.AddDays(90))
                    throw new Exception("Máximo 90 días para pagos programados");

                pago.Estado = EstadoPagoServicio.Programado;
            }
            else
            {
                pago.FechaEjecucion = DateTime.Now;
                pago.Estado = EstadoPagoServicio.Ejecutado;
            }

            // Guardar pago
            var resultado = await pagoServicioDA.realizarPago(pago);

            if (resultado)
            {
                // 1) Generar comprobante solo si NO es programado
                if (!pago.EsProgramado)
                {
                    var comprobante = new Comprobante
                    {
                        ClienteId = pago.ClienteId,
                        CuentaId = pago.CuentaOrigenId,
                        OperacionId = pago.PagoServicioId,
                        Tipo = "PagoServicio",
                        NumeroReferencia = GenerarReferenciaUnicaPago(),
                        Fecha = DateTime.Now
                    };

                    await comprobanteBW.generarComprobante(comprobante);

                    // 2) Registrar transacción en el estado de cuenta
                    await transaccionCuentaBW.registrarTransaccion(new TransaccionCuenta
                    {
                        CuentaId = pago.CuentaOrigenId,
                        ClienteId = pago.ClienteId,
                        Fecha = pago.FechaEjecucion,
                        Monto = -pago.Monto,
                        Descripcion = $"Pago de servicio a proveedor {pago.ProveedorServicioId}",
                        TipoOperacion = TipoOperacionCuenta.PagoServicio,
                        EstadoOperacion = EstadoOperacionCuenta.Exitosa,
                        PagoServicioId = pago.PagoServicioId
                    });
                }

                // 3) Auditoría
                await auditoriaBW.registrarLog(new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.PagoServicio,
                    Entidad = "PagoServicio",
                    EntidadId = pago.PagoServicioId.ToString(),
                    ValoresNuevos = JsonSerializer.Serialize(pago)
                });
            }

            return resultado;
        }

        public async Task<bool> cancelarPago(int id, int usuarioAccionId)
        {
            ReglasDePagoServicio.ValidarId(id);

            var pago = await pagoServicioDA.obtenerPago(id);
            if (pago == null)
                throw new Exception("El pago no existe");

            if (!pago.EsProgramado)
                throw new Exception("Solo pagos programados se pueden cancelar");

            if (pago.Estado != EstadoPagoServicio.Programado)
                throw new Exception("Solo se pueden cancelar pagos programados");

            var limite = pago.FechaEjecucion.AddHours(-24);
            if (DateTime.Now >= limite)
                throw new Exception("Se debe cancelar 24 horas antes");

            var resultado = await pagoServicioDA.cancelarPago(id);

            if (resultado)
            {
                await auditoriaBW.registrarLog(new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CancelacionPago,
                    Entidad = "PagoServicio",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(pago)
                });
            }

            return resultado;
        }

        public Task<PagoServicio> obtenerPago(int id)
        {
            ReglasDePagoServicio.ValidarId(id);
            return pagoServicioDA.obtenerPago(id);
        }

        public Task<List<PagoServicio>> obtenerPagosPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return pagoServicioDA.obtenerPagosPorCliente(clienteId);
        }
    }
}
