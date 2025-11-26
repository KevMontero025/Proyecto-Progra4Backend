using Proyecto.BC.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.BC.ReglasDeNegocios
{
    // Reglas para validar parametros globales del sistema limites, comisiones, etc
    public static class ReglasDeParametroSistema
    {
        public static void Validar(ParametroSistema p)
        {
            if (p == null)
                throw new Exception("El parametro del sistema no puede ser null");

            if (string.IsNullOrWhiteSpace(p.Clave))
                throw new Exception("La clave del parametro es obligatoria");

            if (string.IsNullOrWhiteSpace(p.Valor))
                throw new Exception("El valor del parametro es obligatorio");
        }

        public static void ValidarId(int id)
        {
            if (id <= 0)
                throw new Exception("El id de parametro debe ser mayor a cero");
        }
    }
}
