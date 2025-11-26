

namespace Proyecto.BC.Modelos
{
    // Informacion de un cliente del banco
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string NombreCompleto { get; set; }
        public string Identificacion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int UsuarioId { get; set; } // Relacion con la cuenta de usuario
    }
}
