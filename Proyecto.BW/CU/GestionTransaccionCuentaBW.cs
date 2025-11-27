using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;

namespace Proyecto.BW.CU
{
    public class GestionTransaccionCuentaBW : ITransaccionCuentaBW
    {
        private readonly ITransaccionCuentaDA transaccionDA;

        public GestionTransaccionCuentaBW(ITransaccionCuentaDA transaccionDA)
        {
            this.transaccionDA = transaccionDA;
        }

        public async Task<bool> registrarTransaccion(TransaccionCuenta transaccion)
        {
            ReglasDeTransaccionCuenta.Validar(transaccion);
            return await transaccionDA.registrarTransaccion(transaccion);
        }

        public Task<TransaccionCuenta> obtenerTransaccion(int id)
        {
            ReglasDeTransaccionCuenta.ValidarId(id);
            return transaccionDA.obtenerTransaccion(id);
        }

        public Task<List<TransaccionCuenta>> obtenerTransaccionesPorCuenta(int cuentaId)
        {
            ReglasDeCuenta.ValidarId(cuentaId);
            return transaccionDA.obtenerTransaccionesPorCuenta(cuentaId);
        }

        public Task<List<TransaccionCuenta>> obtenerTransaccionesPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return transaccionDA.obtenerTransaccionesPorCliente(clienteId);
        }

        public async Task<List<TransaccionCuenta>> obtenerHistorial(
            int? clienteId,
            int? cuentaId,
            DateTime? desde,
            DateTime? hasta,
            TipoOperacionCuenta? tipoOperacion,
            string? estadoOperacion)
        {
            List<TransaccionCuenta> baseQuery;

            if (cuentaId.HasValue)
            {
                // admin/gestor consultando por cuenta
                baseQuery = await transaccionDA.obtenerTransaccionesPorCuenta(cuentaId.Value);
            }
            else if (clienteId.HasValue)
            {
                // cliente o admin/gestor filtrando por cliente
                baseQuery = await transaccionDA.obtenerTransaccionesPorCliente(clienteId.Value);
            }
            else
            {
                throw new Exception("Debe indicarse al menos clienteId o cuentaId");
            }

            var query = baseQuery.AsQueryable();

            if (desde.HasValue)
                query = query.Where(t => t.Fecha >= desde.Value);

            if (hasta.HasValue)
                query = query.Where(t => t.Fecha <= hasta.Value);

            if (tipoOperacion.HasValue)
                query = query.Where(t => t.TipoOperacion == tipoOperacion.Value);

            if (!string.IsNullOrWhiteSpace(estadoOperacion))
            {
                if (Enum.TryParse<EstadoOperacionCuenta>(estadoOperacion, true, out var estadoEnum))
                {
                    query = query.Where(t => t.EstadoOperacion == estadoEnum);
                }
            }

            return query
                .OrderByDescending(t => t.Fecha)
                .ToList();
        }
    }
}
