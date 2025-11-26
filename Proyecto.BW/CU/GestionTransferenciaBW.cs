

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System.Text.Json;

namespace Proyecto.BW.CU
{
    public class GestionTransferenciaBW : ITransferenciaBW
    {
        private readonly ITransferenciaDA transferenciaDA;
        private readonly ICuentaDA cuentaDA;
        private readonly IBeneficiarioDA beneficiarioDA;
        private readonly IConsumoLimiteDiarioDA consumoDA;
        private readonly IParametroSistemaDA parametroDA;
        private readonly IAuditoriaBW auditoriaBW;
        private readonly IComprobanteBW comprobanteBW;
        private readonly ITransaccionCuentaBW transaccionCuentaBW;

        public GestionTransferenciaBW(
            ITransferenciaDA transferenciaDA,
            ICuentaDA cuentaDA,
            IBeneficiarioDA beneficiarioDA,
            IConsumoLimiteDiarioDA consumoDA,
            IParametroSistemaDA parametroDA,
            IAuditoriaBW auditoriaBW,
            IComprobanteBW comprobanteBW,
            ITransaccionCuentaBW transaccionCuentaBW)
        {
            this.transferenciaDA = transferenciaDA;
            this.cuentaDA = cuentaDA;
            this.beneficiarioDA = beneficiarioDA;
            this.consumoDA = consumoDA;
            this.parametroDA = parametroDA;
            this.auditoriaBW = auditoriaBW;
            this.comprobanteBW = comprobanteBW;
            this.transaccionCuentaBW = transaccionCuentaBW;
        }

        //Helper para referencia única de transferencias
        private string GenerarReferenciaUnicaTransfer()
        {
            return $"TRF-{Guid.NewGuid():N}";
        }

        public async Task<bool> crearTransferencia(Transferencia transferencia, int usuarioAccionId)
        {
            // Validaciones generales de la entidad
            ReglasDeTransferencia.Validar(transferencia);

            // Cuenta origen
            var cuentaOrigen = await cuentaDA.obtenerCuenta(transferencia.CuentaOrigenId);
            if (cuentaOrigen == null)
                throw new Exception("La cuenta origen no existe");

            if (cuentaOrigen.EstadoCuenta != EstadoCuenta.Activa)
                throw new Exception("La cuenta origen debe estar activa");

            // Destino, propia o tercero
            if (!transferencia.CuentaDestinoId.HasValue && !transferencia.TerceroBeneficiarioId.HasValue)
                throw new Exception("Debe indicar una cuenta destino o un tercero beneficiario");

            if (transferencia.TerceroBeneficiarioId.HasValue)
            {
                var tercero = await beneficiarioDA.obtenerBeneficiario(transferencia.TerceroBeneficiarioId.Value);

                if (tercero == null)
                    throw new Exception("El tercero beneficiario no existe");

                if (tercero.Estado != EstadoBeneficiario.Activo)
                    throw new Exception("El beneficiario no está activo");

                if (!tercero.Confirmado)
                    throw new Exception("El tercero beneficiario debe estar confirmado");
            }

            //Monto y saldo
            if (transferencia.Monto <= 0)
                throw new Exception("El monto de la transferencia debe ser mayor a cero");

            if (cuentaOrigen.Saldo < transferencia.Monto)
                throw new Exception("La cuenta origen no tiene saldo suficiente para la transferencia");

            //Limite diario
            var parametroLimite = await parametroDA.obtenerParametroPorClave("LIMITE_DIARIO_TRANSFERENCIAS");

            decimal limiteDiario = 0m;
            if (parametroLimite != null && !string.IsNullOrWhiteSpace(parametroLimite.Valor))
            {
                if (!decimal.TryParse(parametroLimite.Valor, out limiteDiario))
                    throw new Exception("El valor de LIMITE_DIARIO_TRANSFERENCIAS no es numérico");
            }

            var hoy = DateTime.Today;
            var consumoHoy = await consumoDA.obtenerConsumoPorFecha(transferencia.ClienteId, hoy);

            decimal montoConsumidoHoy = consumoHoy != null
                ? consumoHoy.MontoTotalTransferido
                : 0m;

            if (limiteDiario > 0 && (montoConsumidoHoy + transferencia.Monto > limiteDiario))
                throw new Exception("El monto supera el límite diario disponible para transferencias");

            //Comision
            var parametroComision = await parametroDA.obtenerParametroPorClave("PORCENTAJE_COMISION_TRANSFERENCIA");
            decimal comision = 0m;

            if (parametroComision != null && !string.IsNullOrWhiteSpace(parametroComision.Valor))
            {
                if (!decimal.TryParse(parametroComision.Valor, out var porcentaje))
                    throw new Exception("El valor de PORCENTAJE_COMISION_TRANSFERENCIA no es numérico");

                comision = transferencia.Monto * porcentaje;
            }

            transferencia.SaldoAntes = cuentaOrigen.Saldo;
            transferencia.Comision = comision;
            transferencia.SaldoDespues = cuentaOrigen.Saldo - transferencia.Monto - comision;

            //Programada
            if (transferencia.EsProgramada)
            {
                if (transferencia.FechaEjecucion.Date <= DateTime.Today)
                    throw new Exception("La fecha de ejecución de una transferencia programada debe ser futura");

                if (transferencia.FechaEjecucion.Date > DateTime.Today.AddDays(90))
                    throw new Exception("La transferencia programada no puede superar los 90 días");

                transferencia.Estado = EstadoTransferencia.Programada;
            }
            else
            {
                transferencia.FechaEjecucion = DateTime.Now;
            }

            // IdempotencyKey
            if (string.IsNullOrWhiteSpace(transferencia.IdempotencyKey))
                throw new Exception("La transferencia debe incluir una IdempotencyKey para evitar duplicados");

            var transferenciaExistente = await transferenciaDA.obtenerPorIdempotencyKey(transferencia.IdempotencyKey);
            if (transferenciaExistente != null)
                throw new Exception("Ya existe una transferencia procesada con esta IdempotencyKey.");

            // Umbral de aprobacion
            var parametroUmbral = await parametroDA.obtenerParametroPorClave("UMBRAL_APROBACION_TRANSFERENCIAS");
            decimal umbralAprobacion = 0m;

            if (parametroUmbral != null && !string.IsNullOrWhiteSpace(parametroUmbral.Valor))
            {
                if (!decimal.TryParse(parametroUmbral.Valor, out umbralAprobacion))
                    throw new Exception("El valor de UMBRAL_APROBACION_TRANSFERENCIAS no es numérico");
            }

            if (!transferencia.EsProgramada)
            {
                if (umbralAprobacion > 0 && transferencia.Monto > umbralAprobacion)
                {
                    transferencia.NecesitaAprobacion = true;
                    transferencia.Estado = EstadoTransferencia.PendienteAprobacion;
                }
                else
                {
                    transferencia.NecesitaAprobacion = false;
                    transferencia.Estado = EstadoTransferencia.Exitosa;
                }
            }

            // Guardar
            var resultado = await transferenciaDA.crearTransferencia(transferencia);

            if (resultado)
            {
                //Si ya salio exitosa y no es programada, generar comprobante de una vez
                if (!transferencia.EsProgramada && transferencia.Estado == EstadoTransferencia.Exitosa)
                {
                    var comprobante = new Comprobante
                    {
                        ClienteId = transferencia.ClienteId,
                        CuentaId = transferencia.CuentaOrigenId,
                        OperacionId = transferencia.TransferenciaId,
                        Tipo = "Transferencia",
                        NumeroReferencia = GenerarReferenciaUnicaTransfer(),
                        Fecha = DateTime.Now
                    };

                    await comprobanteBW.generarComprobante(comprobante);
                }

                // Registrar movimiento si fue exitosa
                if (transferencia.Estado == EstadoTransferencia.Exitosa)
                {
                    await transaccionCuentaBW.registrarTransaccion(new TransaccionCuenta
                    {
                        CuentaId = transferencia.CuentaOrigenId,
                        ClienteId = transferencia.ClienteId,
                        Fecha = transferencia.FechaEjecucion,
                        Monto = -(transferencia.Monto + transferencia.Comision),
                        Descripcion = $"Transferencia a {(transferencia.CuentaDestinoId.HasValue ? "cuenta interna" : "tercero")}",
                        TipoOperacion = TipoOperacionCuenta.Transferencia,
                        EstadoOperacion = EstadoOperacionCuenta.Exitosa,
                        TransferenciaId = transferencia.TransferenciaId
                    });
                }

                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CreacionTransferencia,
                    Entidad = "Transferencia",
                    EntidadId = transferencia.TransferenciaId.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(transferencia)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public async Task<bool> cancelarTransferencia(int id, int usuarioAccionId)
        {
            ReglasDeTransferencia.ValidarId(id);

            var tx = await transferenciaDA.obtenerTransferencia(id);
            if (tx == null)
                throw new Exception("La transferencia indicada no existe");

            if (!tx.EsProgramada)
                throw new Exception("Solo se pueden cancelar transferencias programadas");

            if (tx.Estado != EstadoTransferencia.Programada &&
                tx.Estado != EstadoTransferencia.PendienteAprobacion)
                throw new Exception("Solo se pueden cancelar transferencias pendientes");

            var limiteCancelacion = tx.FechaEjecucion.AddHours(-24);
            if (DateTime.Now >= limiteCancelacion)
                throw new Exception("La transferencia solo se puede cancelar hasta 24 horas antes de la ejecucion");

            var resultado = await transferenciaDA.cancelarTransferencia(id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CancelacionTransferencia,
                    Entidad = "Transferencia",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(tx),
                    ValoresNuevos = null
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public async Task<bool> rechazarTransferencia(int id, int usuarioAdminId)
        {
            ReglasDeTransferencia.ValidarId(id);

            var tx = await transferenciaDA.obtenerTransferencia(id);
            if (tx == null)
                throw new Exception("La transferencia no existe");

            if (tx.Estado != EstadoTransferencia.PendienteAprobacion)
                throw new Exception("Solo se pueden rechazar transferencias en estado PendienteAprobacion");

            tx.Estado = EstadoTransferencia.Rechazada;

            var resultado = await transferenciaDA.actualizarTransferencia(tx);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAdminId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.RechazoTransferencia,
                    Entidad = "Transferencia",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(tx),
                    ValoresNuevos = JsonSerializer.Serialize(new { Estado = EstadoTransferencia.Rechazada })
                };

                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public async Task<bool> aprobarTransferencia(int id, int usuarioAdminId)
        {
            ReglasDeTransferencia.ValidarId(id);

            var tx = await transferenciaDA.obtenerTransferencia(id);
            if (tx == null)
                throw new Exception("La transferencia no existe");

            if (tx.Estado != EstadoTransferencia.PendienteAprobacion)
                throw new Exception("Solo se pueden aprobar transferencias en estado PendienteAprobacion");

            tx.Estado = EstadoTransferencia.Exitosa;

            var resultado = await transferenciaDA.actualizarTransferencia(tx);

            if (resultado)
            {
                var comprobante = new Comprobante
                {
                    ClienteId = tx.ClienteId,
                    CuentaId = tx.CuentaOrigenId,
                    OperacionId = tx.TransferenciaId,
                    Tipo = "Transferencia",
                    NumeroReferencia = GenerarReferenciaUnicaTransfer(),
                    Fecha = DateTime.Now
                };

                await comprobanteBW.generarComprobante(comprobante);

                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAdminId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.AprobacionTransferencia,
                    Entidad = "Transferencia",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(tx),
                    ValoresNuevos = JsonSerializer.Serialize(new { Estado = EstadoTransferencia.Exitosa })
                };

                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public Task<Transferencia> obtenerTransferencia(int id)
        {
            ReglasDeTransferencia.ValidarId(id);
            return transferenciaDA.obtenerTransferencia(id);
        }

        public Task<List<Transferencia>> obtenerTransferenciasPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return transferenciaDA.obtenerTransferenciasPorCliente(clienteId);
        }
    }
}
