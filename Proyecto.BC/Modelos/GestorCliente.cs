

namespace Proyecto.BC.Modelos
{
    // Indica que gestor atiende a que cliente
    public class GestorCliente
    {
        public int GestorId { get; set; }  // Usuario con rol Gestor
        public int ClienteId { get; set; } // Cliente asignado a ese gestor
    }
}
