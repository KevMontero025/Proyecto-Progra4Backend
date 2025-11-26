using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferenciaController : ControllerBase
    {
        private readonly ITransferenciaBW transferenciaBW;

        public TransferenciaController(ITransferenciaBW transferenciaBW)
        {
            this.transferenciaBW = transferenciaBW;
        }

        [HttpGet("{id}", Name = "GetTransferenciaById")]
        public async Task<ActionResult<Transferencia>> Get(int id)
        {
            try
            {
                var tx = await transferenciaBW.obtenerTransferencia(id);
                if (tx == null)
                    return NotFound("Transferencia no encontrada");

                return Ok(tx);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCliente/{clienteId}", Name = "GetTransferenciasPorCliente")]
        public async Task<ActionResult<IEnumerable<Transferencia>>> GetPorCliente(int clienteId)
        {
            try
            {
                var lista = await transferenciaBW.obtenerTransferenciasPorCliente(clienteId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost(Name = "CrearTransferencia")]
        public async Task<ActionResult<bool>> Post([FromBody] Transferencia transferencia)
        {
            try
            {
                int usuarioEjecutorId = 1; // Sacar del JWT
                var resultado = await transferenciaBW.crearTransferencia(transferencia, usuarioEjecutorId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Cancelar", Name = "CancelarTransferencia")]
        public async Task<ActionResult<bool>> Cancelar(int id)
        {
            try
            {
                int usuarioEjecutorId = 1; //JWT
                var resultado = await transferenciaBW.cancelarTransferencia(id, usuarioEjecutorId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Aprobar", Name = "AprobarTransferencia")]
        public async Task<ActionResult<bool>> Aprobar(int id)
        {
            try
            {
                int usuarioAdminId = 1; //JWT (admin)
                var resultado = await transferenciaBW.aprobarTransferencia(id, usuarioAdminId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Rechazar", Name = "RechazarTransferencia")]
        public async Task<ActionResult<bool>> Rechazar(int id)
        {
            try
            {
                int usuarioAdminId = 1; //JWT
                var resultado = await transferenciaBW.rechazarTransferencia(id, usuarioAdminId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
