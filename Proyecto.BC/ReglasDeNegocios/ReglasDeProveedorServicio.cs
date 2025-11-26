

using Proyecto.BC.Modelos;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar proveedores de servicios (agua, luz, etc).
    public static class ReglasDeProveedorServicio
    {
        public static void Validar(ProveedorServicio p)
        {
            if (p == null)
                throw new Exception("El proveedor de servicio no puede ser null");

            if (string.IsNullOrWhiteSpace(p.Nombre))
                throw new Exception("El nombre del proveedor es obligatorio");

            if (p.LongitudMinContrato <= 0)
                throw new Exception("La longitud minima del contrato debe ser mayor a cero");

            if (p.LongitudMaxContrato <= 0)
                throw new Exception("La longitud maxima del contrato debe ser mayor a cero");

            if (p.LongitudMaxContrato < p.LongitudMinContrato)
                throw new Exception("La longitud maxima no puede ser menor que la minima");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de proveedor debe ser mayor a cero");
        }
    }
}
