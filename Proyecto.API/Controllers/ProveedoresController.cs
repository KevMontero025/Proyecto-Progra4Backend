using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProveedorServicioController : ControllerBase
    {
        private readonly IProveedorServicioBW proveedorBW;

        public ProveedorServicioController(IProveedorServicioBW proveedorBW)
        {
            this.proveedorBW = proveedorBW;
        }

        [HttpGet(Name = "GetProveedores")]
        public async Task<ActionResult<IEnumerable<ProveedorServicio>>> Get()
        {
            try
            {
                var lista = await proveedorBW.obtenerProveedores();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetProveedorById")]
        public async Task<ActionResult<ProveedorServicio>> Get(int id)
        {
            try
            {
                var proveedor = await proveedorBW.obtenerProveedor(id);
                if (proveedor == null)
                    return NotFound("Proveedor no encontrado");

                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpPost(Name = "RegistrarProveedor")]
        public async Task<ActionResult<bool>> Post([FromBody] ProveedorServicio proveedor)
        {
            try
            {
                var resultado = await proveedorBW.registrarProveedor(proveedor);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "ActualizarProveedor")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] ProveedorServicio proveedor)
        {
            try
            {
                if (id != proveedor.ProveedorServicioId)
                    return BadRequest("El ID del proveedor no coincide con el parámetro");

                var resultado = await proveedorBW.actualizarProveedor(proveedor, id);
                if (!resultado)
                    return NotFound("Proveedor no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "EliminarProveedor")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var resultado = await proveedorBW.eliminarProveedor(id);
                if (!resultado)
                    return NotFound("Proveedor no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
