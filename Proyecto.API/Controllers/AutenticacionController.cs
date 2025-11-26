using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BW.Interfaces.BW;
using Proyecto.API.Models;

namespace Proyecto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacionController : ControllerBase
    {
        private readonly IAutenticacionBW autenticacionBW;

        public AutenticacionController(IAutenticacionBW autenticacionBW)
        {
            this.autenticacionBW = autenticacionBW;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await autenticacionBW.Login(request.Email, request.Password);

                return Ok(new
                {
                    token
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { mensaje = ex.Message });
            }
        }
    }
}
