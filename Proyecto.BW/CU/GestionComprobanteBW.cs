

using Proyecto.BC.Modelos;
using Proyecto.BC.ReglasDeNegocios;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;

namespace Proyecto.BW.CU
{
    public class GestionComprobanteBW : IComprobanteBW
    {
        private readonly IComprobanteDA comprobanteDA;

        public GestionComprobanteBW(IComprobanteDA comprobanteDA)
        {
            this.comprobanteDA = comprobanteDA;
        }

        public async Task<bool> generarComprobante(Comprobante comprobante)
        {
            ReglasDeComprobante.Validar(comprobante);
            return await comprobanteDA.generarComprobante(comprobante);
        }

        public Task<Comprobante> obtenerComprobante(int id)
        {
            ReglasDeComprobante.ValidarId(id);
            return comprobanteDA.obtenerComprobante(id);
        }

        public Task<List<Comprobante>> obtenerComprobantesPorCliente(int clienteId)
        {
            ReglasDeCliente.ValidarId(clienteId);
            return comprobanteDA.obtenerComprobantesPorCliente(clienteId);
        }
    }
}
