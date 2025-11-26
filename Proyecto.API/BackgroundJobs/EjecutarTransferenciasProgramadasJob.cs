using Proyecto.BC.Modelos.Enum;
using Proyecto.BW.Interfaces.DA;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Proyecto.API.BackgroundJobs
{
    public class EjecutarTransferenciasProgramadasJob : BackgroundService
    {
        private readonly ITransferenciaDA transferenciaDA;
        private readonly ICuentaDA cuentaDA;
        private readonly ILogger<EjecutarTransferenciasProgramadasJob> _logger;

        public EjecutarTransferenciasProgramadasJob(
            ITransferenciaDA transferenciaDA,
            ICuentaDA cuentaDA,
            ILogger<EjecutarTransferenciasProgramadasJob> logger)
        {
            this.transferenciaDA = transferenciaDA;
            this.cuentaDA = cuentaDA;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Job ejecutándose…");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcesarTransferenciasProgramadas();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcesarTransferenciasProgramadas()
        {
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
                catch
                {
                    tx.Estado = EstadoTransferencia.Fallida;
                    await transferenciaDA.actualizarTransferencia(tx);
                }
            }
        }
    }
}
