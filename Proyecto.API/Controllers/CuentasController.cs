using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BW.Interfaces.BW;

namespace Proyecto.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaBW cuentaBW;

        public CuentaController(ICuentaBW cuentaBW)
        {
            this.cuentaBW = cuentaBW;
        }

        [HttpGet("{id}", Name = "GetCuentaById")]
        public async Task<ActionResult<Cuenta>> Get(int id)
        {
            try
            {
                var cuenta = await cuentaBW.obtenerCuenta(id);
                if (cuenta == null)
                    return NotFound("Cuenta no encontrada");

                return Ok(cuenta);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("PorCliente/{clienteId}", Name = "GetCuentasPorCliente")]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetPorCliente(int clienteId)
        {
            try
            {
                var cuentas = await cuentaBW.obtenerCuentasPorCliente(clienteId);
                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("Filtrar", Name = "FiltrarCuentas")]
        public async Task<ActionResult<IEnumerable<Cuenta>>> Filtrar(
            int usuarioId,
            int? clienteId = null,
            TipoCuenta? tipo = null,
            Moneda? moneda = null,
            EstadoCuenta? estado = null)
        {
            try
            {
                var cuentas = await cuentaBW.filtrarCuentas(usuarioId, clienteId, tipo, moneda, estado);
                return Ok(cuentas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "CrearCuenta")]
        public async Task<ActionResult<bool>> Post([FromBody] Cuenta cuenta)
        {
            try
            {
                // Mientras no tengas JWT
                int usuarioCreadorId = 1; // Cambiar por ID de usuario autenticado

                var resultado = await cuentaBW.crearCuenta(cuenta, usuarioCreadorId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Bloquear", Name = "BloquearCuenta")]
        public async Task<ActionResult<bool>> Bloquear(int id)
        {
            try
            {
                int usuarioAdminId = 1; // Sacar del token JWT

                var resultado = await cuentaBW.bloquearCuenta(id, usuarioAdminId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/Cerrar", Name = "CerrarCuenta")]
        public async Task<ActionResult<bool>> Cerrar(int id)
        {
            try
            {
                int usuarioAdminId = 1; // Sacar del token JWT

                var resultado = await cuentaBW.cerrarCuenta(id, usuarioAdminId);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}