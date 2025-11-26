
namespace Proyecto.BC.Modelos
{
    // Parámetros globales del banco límites, comisiones, configuraciones
    //Sirve para guardar configuraciones que afectan las reglas del negocio, pero que pueden cambiar sin tocar el código
    public class ParametroSistema
    {
        public int ParametroSistemaId { get; set; }

        public string Clave { get; set; }   // Nombre del parámetro
        public string Valor { get; set; }   // Valor almacenado
        public string Descripcion { get; set; } // Explicación del parámetro
    }
}
