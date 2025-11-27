using Proyecto.BC.Modelos.Enum;
using Proyecto.BW.Interfaces.DA;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Proyecto.API.BackgroundJobs
{
    public class EjecutarTransferenciasProgramadasJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<EjecutarTransferenciasProgramadasJob> _logger;

        public EjecutarTransferenciasProgramadasJob(
            IServiceScopeFactory scopeFactory,
            ILogger<EjecutarTransferenciasProgramadasJob> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Job de transferencias programadas iniciado...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcesarTransferenciasProgramadas();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al procesar transferencias programadas");
                }

                // Esperar 1 minuto antes de la siguiente ejecución
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcesarTransferenciasProgramadas()
        {
            // Creamos un scope nuevo para obtener servicios scoped (DbContext, DA, etc.)
            using var scope = _scopeFactory.CreateScope();

            var transferenciaDA = scope.ServiceProvider.GetRequiredService<ITransferenciaDA>();
            var cuentaDA = scope.ServiceProvider.GetRequiredService<ICuentaDA>();

            var pendientes = await transferenciaDA.obtenerTransferenciasProgramadasParaEjecutar();

            foreach (var tx in pendientes)
            {
                try
                {
                    var cuenta = await cuentaDA.obtenerCuenta(tx.CuentaOrigenId);

                    if (cuenta.Saldo < (tx.Monto + tx.Comision))
                    {
                        tx.Estado = EstadoTransferencia.Fallida;
                    }
                    else
                    {
                        cuenta.Saldo -= (tx.Monto + tx.Comision);
                        tx.Estado = EstadoTransferencia.Exitosa;
                    }

                    await transferenciaDA.actualizarTransferencia(tx);
                }
                catch (Exception)
                {
                    tx.Estado = EstadoTransferencia.Fallida;
                    await transferenciaDA.actualizarTransferencia(tx);
                }
            }
        }
    }
}
