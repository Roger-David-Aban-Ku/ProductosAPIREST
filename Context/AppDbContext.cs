using Microsoft.EntityFrameworkCore;
using ProductosAPIREST.Models;

namespace ProductosAPIREST.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Producto> Productos{ get; set; }
    }
}
