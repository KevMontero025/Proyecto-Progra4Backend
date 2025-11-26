using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransaccionCuentaController : ControllerBase
    {
        private readonly ITransaccionCuentaBW transaccionCuentaBW;

        public TransaccionCuentaController(ITransaccionCuentaBW transaccionCuentaBW)
        {
            this.transaccionCuentaBW = transaccionCuentaBW;
        }

        [HttpGet("{id}", Name = "GetTransaccionCuentaById")]
        public async Task<ActionResult<TransaccionCuenta>> Get(int id)
        {
            try
            {
                var tx = await transaccionCuentaBW.obtenerTransaccion(id);
                if (tx == null)
                    return NotFound("Transacción no encontrada");

                return Ok(tx);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCuenta/{cuentaId}", Name = "GetTransaccionesPorCuenta")]
        public async Task<ActionResult<IEnumerable<TransaccionCuenta>>> GetPorCuenta(int cuentaId)
        {
            try
            {
                var lista = await transaccionCuentaBW.obtenerTransaccionesPorCuenta(cuentaId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCliente/{clienteId}", Name = "GetTransaccionesPorCliente")]
        public async Task<ActionResult<IEnumerable<TransaccionCuenta>>> GetPorCliente(int clienteId)
        {
            try
            {
                var lista = await transaccionCuentaBW.obtenerTransaccionesPorCliente(clienteId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
