using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ComprobanteController : ControllerBase
    {
        private readonly IComprobanteBW comprobanteBW;

        public ComprobanteController(IComprobanteBW comprobanteBW)
        {
            this.comprobanteBW = comprobanteBW;
        }

        [HttpGet("{id}", Name = "GetComprobanteById")]
        public async Task<ActionResult<Comprobante>> Get(int id)
        {
            try
            {
                var comp = await comprobanteBW.obtenerComprobante(id);
                if (comp == null)
                    return NotFound("Comprobante no encontrado");

                return Ok(comp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCliente/{clienteId}", Name = "GetComprobantesPorCliente")]
        public async Task<ActionResult<IEnumerable<Comprobante>>> GetPorCliente(int clienteId)
        {
            try
            {
                var lista = await comprobanteBW.obtenerComprobantesPorCliente(clienteId);
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
