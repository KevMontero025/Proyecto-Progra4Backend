

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar beneficiarios de terceros.
    public static class ReglasDeTerceroBeneficiario
    {
        public static void Validar(TerceroBeneficiario b)
        {
            if (b == null)
                throw new Exception("El beneficiario no puede ser null");

            if (b.ClienteId <= 0)
                throw new Exception("El beneficiario debe pertenecer a un cliente valido");

            if (string.IsNullOrWhiteSpace(b.Alias))
                throw new Exception("El alias del beneficiario es obligatorio");

            if (b.Alias.Length < 3 || b.Alias.Length > 30)
                throw new Exception("El alias del beneficiario debe tener entre 3 y 30 caracteres");

            if (string.IsNullOrWhiteSpace(b.Banco))
                throw new Exception("El banco del beneficiario es obligatorio");

            if (string.IsNullOrWhiteSpace(b.NumeroCuenta))
                throw new Exception("El numero de cuenta del beneficiario es obligatorio");

            if (b.NumeroCuenta.Length < 12 || b.NumeroCuenta.Length > 20)
                throw new Exception("El numero de cuenta debe tener entre 12 y 20 digitos");

            if (!b.NumeroCuenta.All(char.IsDigit))
                throw new Exception("El numero de cuenta debe contener solo digitos");

            if (string.IsNullOrWhiteSpace(b.Pais))
                throw new Exception("El pais del beneficiario es obligatorio");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de beneficiario debe ser mayor a cero");
        }
    }
}