

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar una cuenta bancaria.
    public static class ReglasDeCuenta
    {
        public static void ValidarCreacion(Cuenta cuenta)
        {
            if (cuenta == null)
                throw new Exception("La cuenta no puede ser null");

            if (string.IsNullOrWhiteSpace(cuenta.NumeroCuenta))
                throw new Exception("El numero de cuenta es obligatorio");

            if (cuenta.NumeroCuenta.Length != 12)
                throw new Exception("El numero de cuenta debe tener 12 digitos");

            if (cuenta.Saldo < 0)
                throw new Exception("El saldo inicial no puede ser negativo");

            if (cuenta.ClienteId <= 0)
                throw new Exception("La cuenta debe pertenecer a un cliente valido");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de cuenta debe ser mayor a cero");
        }

        //Validar reglas para cerrar una cuenta
        public static void ValidarCierre(Cuenta cuenta)
        {
            if (cuenta == null)
                throw new Exception("La cuenta no existe");

            if (cuenta.EstadoCuenta == EstadoCuenta.Cerrada)
                throw new Exception("La cuenta ya esta cerrada");

            if (cuenta.EstadoCuenta == EstadoCuenta.Bloqueada)
                throw new Exception("No se puede cerrar una cuenta bloqueada");

            if (cuenta.Saldo > 0)
                throw new Exception("No se puede cerrar una cuenta con saldo mayor a cero");
        }

        //Validar reglas para bloquear una cuenta
        public static void ValidarBloqueo(Cuenta cuenta)
        {
            if (cuenta == null)
                throw new Exception("La cuenta no existe");

            if (cuenta.EstadoCuenta == EstadoCuenta.Cerrada)
                throw new Exception("No se puede bloquear una cuenta cerrada");

            if (cuenta.EstadoCuenta == EstadoCuenta.Bloqueada)
                throw new Exception("La cuenta ya esta bloqueada");
        }
    }
}
