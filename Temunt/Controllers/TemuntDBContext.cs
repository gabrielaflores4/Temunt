using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Temunt.Models;


namespace Temunt.Controllers
{
    public class TemuntDbContext : DbContext
    {
        public TemuntDbContext(DbContextOptions<TemuntDbContext> options) : base(options) { }

        public DbSet<clientes> Clientes { get; set; }

        public DbSet<usuarios> usuarios { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<pedidos> pedidos { get; set; }
        public DbSet<estadoP> estadoP { get; set; }
        public DbSet<proveedores> proveedores { get; set; }
        public DbSet<producto> producto { get; set; }
        public DbSet<detalleP> detalleP { get; set; }
        public DbSet<notificaciones> notificaciones { get; set; }
        public DbSet<clientes> clientes { get; set; }
    }
}
