using Microsoft.EntityFrameworkCore;
using Venta.API.Data.Entities;

namespace Venta.API.Data
{
    public class ApplicationDbContext : DbContext
    {



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Entities.Venta> Ventas { get; set; }
        public DbSet<VentaConcepto> VentaConceptos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VentaConcepto>()
                .Property(vc => vc.ValorUnitario)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Entities.Venta>()
                .Property(v => v.Total)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Producto>()
                .Property(p => p.Precio)
                .HasColumnType("decimal(18,2)");
        }
    }
}
