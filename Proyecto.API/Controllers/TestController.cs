// En Proyecto.API/Controllers/TestController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.DA.Config;

namespace Proyecto.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly BancoContext _context;

        public TestController(BancoContext context)
        {
            _context = context;
        }

        [HttpPost("crear-datos-prueba")]
        public async Task<IActionResult> CrearDatosPrueba()
        {
            // Crear usuarios
            var usuario1 = new Usuario
            {
                Email = "cliente1@test.com",
                PasswordHash = "pass123",
                Rol = RolUsuario.Cliente
            };
            var usuario2 = new Usuario
            {
                Email = "cliente2@test.com",
                PasswordHash = "pass123",
                Rol = RolUsuario.Cliente
            };

            _context.Usuario.AddRange(usuario1, usuario2);
            await _context.SaveChangesAsync();

            // Crear clientes
            var cliente1 = new Cliente
            {
                NombreCompleto = "Cliente Uno",
                Identificacion = "111111111",
                Telefono = "8888-8888",
                Correo = "cliente1@test.com",
                UsuarioId = usuario1.UsuarioId
            };
            var cliente2 = new Cliente
            {
                NombreCompleto = "Cliente Dos",
                Identificacion = "222222222",
                Telefono = "9999-9999",
                Correo = "cliente2@test.com",
                UsuarioId = usuario2.UsuarioId
            };

            _context.Cliente.AddRange(cliente1, cliente2);
            await _context.SaveChangesAsync();

            // Crear cuentas
            var cuenta1 = new Cuenta
            {
                NumeroCuenta = "123456789012",
                TipoCuenta = TipoCuenta.Ahorros,
                Moneda = Moneda.CRC,
                Saldo = 100000,
                EstadoCuenta = EstadoCuenta.Activa,
                ClienteId = cliente1.ClienteId
            };
            var cuenta2 = new Cuenta
            {
                NumeroCuenta = "987654321098",
                TipoCuenta = TipoCuenta.Ahorros,
                Moneda = Moneda.CRC,
                Saldo = 50000,
                EstadoCuenta = EstadoCuenta.Activa,
                ClienteId = cliente2.ClienteId
            };

            _context.Cuenta.AddRange(cuenta1, cuenta2);
            await _context.SaveChangesAsync();

            // Crear parámetros del sistema
            var parametros = new List<ParametroSistema>
            {
                new ParametroSistema { Clave = "LIMITE_DIARIO_TRANSFERENCIAS", Valor = "500000", Descripcion = "Límite diario de transferencias por cliente" },
                new ParametroSistema { Clave = "PORCENTAJE_COMISION_TRANSFERENCIA", Valor = "0.01", Descripcion = "1% de comisión por transferencia" },
                new ParametroSistema { Clave = "UMBRAL_APROBACION_TRANSFERENCIAS", Valor = "1000000", Descripcion = "Transferencias mayores a 1,000,000 requieren aprobación" }
            };

            _context.ParametroSistema.AddRange(parametros);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Datos de prueba creados exitosamente",
                cuentaOrigen = new { id = cuenta1.CuentaId, numero = cuenta1.NumeroCuenta, saldo = cuenta1.Saldo },
                cuentaDestino = new { id = cuenta2.CuentaId, numero = cuenta2.NumeroCuenta, saldo = cuenta2.Saldo }
            });
        }
    }
}