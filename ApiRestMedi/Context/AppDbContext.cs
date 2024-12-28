using ApiRestMedi.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRestMedi.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
                
        }
        public DbSet<Productos> Productos { get; set; } 
        public DbSet<Categorias> Categorias { get; set; }
    }
}
