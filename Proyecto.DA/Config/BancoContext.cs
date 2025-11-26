using Microsoft.EntityFrameworkCore;
using Proyecto.BC.Modelos;


namespace Proyecto.DA.Config
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options): base(options){ }
        //  TABLAS / ENTIDADES

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<TerceroBeneficiario> TerceroBeneficiario { get; set; }
        public DbSet<ProveedorServicio> ProveedorServicio { get; set; }
        public DbSet<PagoServicio> PagoServicio { get; set; }
        public DbSet<ParametroSistema> ParametroSistema { get; set; }
        public DbSet<ConsumoLimiteDiario> ConsumoLimiteDiario { get; set; }
        public DbSet<GestorCliente> GestorCliente { get; set; }
        public DbSet<TransaccionCuenta> TransaccionCuenta { get; set; }
        public DbSet<Transferencia> Transferencia { get; set; }
        public DbSet<Comprobante> Comprobante { get; set; }
        public DbSet<LogAuditoria> LogAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
