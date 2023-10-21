using Microsoft.EntityFrameworkCore;

namespace UdeM_Bank_MyK
{
    internal class UdemBankContext: DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<GrupoAhorro> GruposAhorros { get; set; }
        public DbSet<GrupoAhorroXCliente> GrupoAhorroXCliente { get; set; }
        public DbSet<MovimientosGrupoAhorroXCliente> Movimientos { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlite($"Data Source = UdeM_Bank_MyK.db");
    }
}