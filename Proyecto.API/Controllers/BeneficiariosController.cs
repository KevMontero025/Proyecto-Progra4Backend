using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BeneficiarioController : ControllerBase
    {
        private readonly IBeneficiarioBW beneficiarioBW;

        public BeneficiarioController(IBeneficiarioBW beneficiarioBW)
        {
            this.beneficiarioBW = beneficiarioBW;
        }

        [HttpGet("{id}", Name = "GetBeneficiarioById")]
        public async Task<ActionResult<TerceroBeneficiario>> Get(int id)
        {
            try
            {
                var beneficiario = await beneficiarioBW.obtenerBeneficiario(id);
                if (beneficiario == null)
                    return NotFound("Beneficiario no encontrado");

                return Ok(beneficiario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCliente/{clienteId}", Name = "GetBeneficiariosPorCliente")]
        public async Task<ActionResult<IEnumerable<TerceroBeneficiario>>> GetPorCliente(int clienteId)
        {
            try
            {
                var beneficiarios = await beneficiarioBW.obtenerBeneficiariosPorCliente(clienteId);
                return Ok(beneficiarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost(Name = "RegistrarBeneficiario")]
        public async Task<ActionResult<bool>> Post([FromBody] TerceroBeneficiario beneficiario)
        {
            try
            {
                var resultado = await beneficiarioBW.registrarBeneficiario(beneficiario);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "ActualizarBeneficiario")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] TerceroBeneficiario beneficiario)
        {
            try
            {
                if (id != beneficiario.TerceroBeneficiarioId)
                    return BadRequest("El ID del beneficiario no coincide con el parámetro");

                var resultado = await beneficiarioBW.actualizarBeneficiario(beneficiario, id);
                if (!resultado)
                    return NotFound("Beneficiario no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "EliminarBeneficiario")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var resultado = await beneficiarioBW.eliminarBeneficiario(id);
                if (!resultado)
                    return NotFound("Beneficiario no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
