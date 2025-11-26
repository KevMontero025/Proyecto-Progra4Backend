

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System.Text.Json;

namespace Proyecto.BW.CU
{
    public class GestionParametroSistemaBW : IParametroSistemaBW
    {
        private readonly IParametroSistemaDA parametroDA;
        private readonly IAuditoriaBW auditoriaBW;

        public GestionParametroSistemaBW(IParametroSistemaDA parametroDA, IAuditoriaBW auditoriaBW)
        {
            this.parametroDA = parametroDA;
            this.auditoriaBW = auditoriaBW;
        }

        public async Task<bool> registrarParametro(ParametroSistema parametro, int usuarioEjecutorId)
        {
            ReglasDeParametroSistema.Validar(parametro);

            var resultado = await parametroDA.registrarParametro(parametro);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioEjecutorId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CreacionParametroSistema,
                    Entidad = "ParametroSistema",
                    EntidadId = parametro.ParametroSistemaId.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(parametro)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public async Task<bool> actualizarParametro(ParametroSistema parametro, int id, int usuarioEjecutorId)
        {
            ReglasDeParametroSistema.ValidarId(id);
            ReglasDeParametroSistema.Validar(parametro);

            var resultado = await parametroDA.actualizarParametro(parametro, id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioEjecutorId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.ActualizacionParametroSistema,
                    Entidad = "ParametroSistema",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(parametro)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        public Task<ParametroSistema> obtenerParametroPorClave(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave))
                throw new Exception("La clave del parametro no puede ser vacia");
            return parametroDA.obtenerParametroPorClave(clave);
        }

        public Task<List<ParametroSistema>> obtenerParametros()
        {
            return parametroDA.obtenerParametros();
        }
    }
}