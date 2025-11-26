using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PagoServicioController : ControllerBase
    {
        private readonly IPagoServicioBW pagoServicioBW;

        public PagoServicioController(IPagoServicioBW pagoServicioBW)
        {
            this.pagoServicioBW = pagoServicioBW;
        }

        [HttpGet("{id}", Name = "GetPagoServicioById")]
        public async Task<ActionResult<PagoServicio>> Get(int id)
        {
            try
            {
                var pago = await pagoServicioBW.obtenerPago(id);
                if (pago == null)
                    return NotFound("Pago no encontrado");

                return Ok(pago);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCliente/{clienteId}", Name = "GetPagosPorCliente")]
        public async Task<ActionResult<IEnumerable<PagoServicio>>> GetPorCliente(int clienteId)
        {
            try
            {
                var lista = await pagoServicioBW.obtenerPagosPorCliente(clienteId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost(Name = "RealizarPagoServicio")]
        public async Task<ActionResult<bool>> Post([FromBody] PagoServicio pago)
        {
            try
            {
                int usuarioEjecutorId = 1; //JWT
                var resultado = await pagoServicioBW.realizarPago(pago, usuarioEjecutorId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Cancelar", Name = "CancelarPagoServicio")]
        public async Task<ActionResult<bool>> Cancelar(int id)
        {
            try
            {
                int usuarioEjecutorId = 1; //JWT
                var resultado = await pagoServicioBW.cancelarPago(id, usuarioEjecutorId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}