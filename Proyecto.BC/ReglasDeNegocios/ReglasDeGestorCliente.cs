

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para la relacion entre un gestor y un cliente.
    public static class ReglasDeGestorCliente
    {
        public static void Validar(GestorCliente relacion)
        {
            if (relacion == null)
                throw new Exception("La relacion gestor-cliente no puede ser null");

            if (relacion.GestorId <= 0)
                throw new Exception("El id del gestor debe ser mayor a cero");

            if (relacion.ClienteId <= 0)
                throw new Exception("El id del cliente debe ser mayor a cero");
        }
    }
}
