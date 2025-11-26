
namespace Proyecto.BC.Modelos
{
    // Empresas para pagos de servicios
    public class ProveedorServicio
    {
        public int ProveedorServicioId { get; set; }

        public string Nombre { get; set; }
        public int LongitudMinContrato { get; set; }  // Para validar contrato
        public int LongitudMaxContrato { get; set; }
        public string ReglasAdicionales { get; set; } // Validaciones extra
    }
}
