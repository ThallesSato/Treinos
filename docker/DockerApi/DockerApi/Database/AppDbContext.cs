using DockerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerApi.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
     public DbSet<Cliente> Clientes {get; set;}
}