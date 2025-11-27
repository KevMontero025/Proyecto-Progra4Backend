using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProveedorServicioController : ControllerBase
    {
        private readonly IProveedorServicioBW proveedorServicioBW;

        public ProveedorServicioController(IProveedorServicioBW proveedorServicioBW)
        {
            this.proveedorServicioBW = proveedorServicioBW;
        }

        [HttpGet("{id}", Name = "GetProveedorById")]
        public async Task<ActionResult<ProveedorServicio>> Get(int id)
        {
            try
            {
                var proveedor = await proveedorServicioBW.obtenerProveedor(id);
                if (proveedor == null)
                    return NotFound("Proveedor no encontrado");

                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet(Name = "GetProveedores")]
        public async Task<ActionResult<IEnumerable<ProveedorServicio>>> Get()
        {
            try
            {
                var proveedores = await proveedorServicioBW.obtenerProveedores();
                return Ok(proveedores);
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
                int usuarioAccionId = 1; // luego vendrá del JWT

                var resultado = await proveedorServicioBW.registrarProveedor(proveedor, usuarioAccionId);
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

                int usuarioAccionId = 1;

                var resultado = await proveedorServicioBW.actualizarProveedor(proveedor, id, usuarioAccionId);
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
                int usuarioAccionId = 1;

                var resultado = await proveedorServicioBW.eliminarProveedor(id, usuarioAccionId);
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
