using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioBW usuarioBW;

        public UsuarioController(IUsuarioBW usuarioBW)
        {
            this.usuarioBW = usuarioBW;
        }

        [HttpGet(Name = "GetUsuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> Get()
        {
            try
            {
                var usuarios = await usuarioBW.obtenerUsuarios();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno en el servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetUsuarioById")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            try
            {
                var usuario = await usuarioBW.obtenerUsuario(id);
                if (usuario == null)
                    return NotFound("Usuario no encontrado");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno en el servidor: {ex.Message}");
            }
        }

        [HttpPost(Name = "RegistrarUsuario")]
        public async Task<ActionResult<bool>> Post([FromBody] Usuario usuario)
        {
            try
            {
                var resultado = await usuarioBW.registrarUsuario(usuario);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}", Name = "ActualizarUsuario")]
        public async Task<ActionResult<bool>> Put(int id, [FromBody] Usuario usuario)
        {
            try
            {
                if (id != usuario.UsuarioId)
                    return BadRequest("El ID del usuario no coincide con el parámetro proporcionado");

                var resultado = await usuarioBW.actualizarUsuario(usuario, id);

                if (!resultado)
                    return NotFound("Usuario no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "EliminarUsuario")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            try
            {
                var resultado = await usuarioBW.eliminarUsuario(id);
                if (!resultado)
                    return NotFound("Usuario no encontrado");

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}