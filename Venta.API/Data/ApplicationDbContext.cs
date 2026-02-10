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

    }
}
