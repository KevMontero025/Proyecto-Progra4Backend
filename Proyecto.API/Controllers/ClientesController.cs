using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteBW clienteBW;

        public ClienteController(IClienteBW clienteBW)
        {
            this.clienteBW = clienteBW;
        }

        [HttpGet(Name = "GetClientes")]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get()
        {
            try
            {
                var clientes = await clienteBW.obtenerClientes();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetClienteById")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            try
            {
                var cliente = await clienteBW.obtenerCliente(id);
                if (cliente == null)
                    return NotFound("Cliente no encontrado");

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost(Name = "RegistrarCliente")]
        public async Task<ActionResult<bool>> Post([FromBody] Cliente cliente)
        {
            try
            {
                var resultado = await clienteBW.registrarCliente(cliente);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "ActualizarCliente")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] Cliente cliente)
        {
            try
            {
                if (id != cliente.ClienteId)
                    return BadRequest("El ID del cliente no coincide con el parámetro proporcionado");

                var resultado = await clienteBW.actualizarCliente(cliente, id);

                if (!resultado)
                    return NotFound("Cliente no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "EliminarCliente")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var resultado = await clienteBW.eliminarCliente(id);
                if (!resultado)
                    return NotFound("Cliente no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
