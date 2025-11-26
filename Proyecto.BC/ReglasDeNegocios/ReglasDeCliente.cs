

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar los datos de un cliente del banco.
    public static class ReglasDeCliente
    {
        public static void Validar(Cliente cliente)
        {
            if (cliente == null)
                throw new Exception("El cliente no puede ser null");

            if (string.IsNullOrWhiteSpace(cliente.NombreCompleto))
                throw new Exception("El nombre del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.Identificacion))
                throw new Exception("La identificacion del cliente es obligatoria");

            if (string.IsNullOrWhiteSpace(cliente.Telefono))
                throw new Exception("El telefono del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.Correo))
                throw new Exception("El correo del cliente es obligatorio");

            if (cliente.UsuarioId <= 0)
                throw new Exception("El cliente debe estar asociado a un usuario valido");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de cliente debe ser mayor a cero");
        }
    }
}
