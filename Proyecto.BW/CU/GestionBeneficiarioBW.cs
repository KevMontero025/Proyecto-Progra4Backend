

using Proyecto.BC.Modelos;
using Proyecto.BC.Modelos.Enum;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using System.Text.Json;

namespace Proyecto.BW.CU
{
    public class GestionBeneficiarioBW : IBeneficiarioBW
    {
        private readonly IBeneficiarioDA beneficiarioDA;
        private readonly ITransferenciaDA transferenciaDA;
        private readonly IAuditoriaBW auditoriaBW;

        public GestionBeneficiarioBW(
            IBeneficiarioDA beneficiarioDA,
            ITransferenciaDA transferenciaDA,
            IAuditoriaBW auditoriaBW)
        {
            this.beneficiarioDA = beneficiarioDA;
            this.transferenciaDA = transferenciaDA;
            this.auditoriaBW = auditoriaBW;
        }

        // Registro de terceros
        public async Task<bool> registrarBeneficiario(TerceroBeneficiario beneficiario, int usuarioAccionId)
        {
            ReglasDeTerceroBeneficiario.Validar(beneficiario);

            // Alias único por cliente
            var beneficiariosCliente = await beneficiarioDA.obtenerBeneficiariosPorCliente(beneficiario.ClienteId);

            bool aliasRepetido = beneficiariosCliente
                .Any(b => b.Alias.Trim().ToLower() == beneficiario.Alias.Trim().ToLower());

            if (aliasRepetido)
                throw new Exception("Ya existe un beneficiario con ese alias para este cliente");

            // Los terceros se crean como Inactivos 
            beneficiario.Estado = EstadoBeneficiario.Inactivo;
            beneficiario.Confirmado = false;

            var resultado = await beneficiarioDA.registrarBeneficiario(beneficiario);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.CreacionBeneficiario,
                    Entidad = "TerceroBeneficiario",
                    EntidadId = beneficiario.TerceroBeneficiarioId.ToString(),
                    ValoresAnteriores = null,
                    ValoresNuevos = JsonSerializer.Serialize(beneficiario)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // Edición de terceros
        public async Task<bool> actualizarBeneficiario(TerceroBeneficiario beneficiario, int id, int usuarioAccionId)
        {
            ReglasDeTerceroBeneficiario.ValidarId(id);
            ReglasDeTerceroBeneficiario.Validar(beneficiario);

            // Cargar estado anterior para auditar
            var beneficiarioAnterior = await beneficiarioDA.obtenerBeneficiario(id);
            if (beneficiarioAnterior == null)
                throw new Exception("El beneficiario indicado no existe");

            // Validar alias único al actualizar
            var beneficiariosCliente = await beneficiarioDA.obtenerBeneficiariosPorCliente(beneficiario.ClienteId);

            bool aliasRepetido = beneficiariosCliente
                .Any(b => b.TerceroBeneficiarioId != id &&
                          b.Alias.Trim().ToLower() == beneficiario.Alias.Trim().ToLower());

            if (aliasRepetido)
                throw new Exception("Ya existe otro beneficiario con ese alias para este cliente");

            var resultado = await beneficiarioDA.actualizarBeneficiario(beneficiario, id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.ActualizacionBeneficiario,
                    Entidad = "TerceroBeneficiario",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(beneficiarioAnterior),
                    ValoresNuevos = JsonSerializer.Serialize(beneficiario)
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // Eliminacion ,no se puede si tiene operaciones programadas pendientes
        public async Task<bool> eliminarBeneficiario(int id, int usuarioAccionId)
        {
            ReglasDeTerceroBeneficiario.ValidarId(id);

            // No se puede eliminar si tiene transferencias programadas pendientes
            var transferenciasProgramadas =
                await transferenciaDA.obtenerTransferenciasProgramadasPorBeneficiario(id);

            if (transferenciasProgramadas.Any())
                throw new Exception("No se puede eliminar el beneficiario porque tiene transferencias programadas pendientes");

            // Cargar el estado anterior para auditar
            var beneficiarioAnterior = await beneficiarioDA.obtenerBeneficiario(id);
            if (beneficiarioAnterior == null)
                throw new Exception("El beneficiario indicado no existe");

            var resultado = await beneficiarioDA.eliminarBeneficiario(id);

            if (resultado)
            {
                var log = new LogAuditoria
                {
                    UsuarioId = usuarioAccionId,
                    Fecha = DateTime.Now,
                    TipoOperacion = TipoOperacionAuditoria.EliminacionBeneficiario,
                    Entidad = "TerceroBeneficiario",
                    EntidadId = id.ToString(),
                    ValoresAnteriores = JsonSerializer.Serialize(beneficiarioAnterior),
                    ValoresNuevos = null
                };

                ReglasDeLogAuditoria.Validar(log);
                await auditoriaBW.registrarLog(log);
            }

            return resultado;
        }

        // Consulta de terceros de un cliente
        public Task<TerceroBeneficiario> obtenerBeneficiario(int id)
        {
            ReglasDeTerceroBeneficiario.ValidarId(id);
            return beneficiarioDA.obtenerBeneficiario(id);
        }

        public Task<List<TerceroBeneficiario>> obtenerBeneficiariosPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return beneficiarioDA.obtenerBeneficiariosPorCliente(clienteId);
        }
    }
}
