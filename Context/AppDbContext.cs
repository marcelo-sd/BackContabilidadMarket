using ContabilidaMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ContabilidaMarket.Context
{
    public class AppDbContext : DbContext
    {
        private readonly string _coneccionString;
        public AppDbContext(DbContextOptions<AppDbContext> Options, IConfiguration configuration) : base(Options)
        {
            _coneccionString = configuration.GetConnectionString("cadenaSql");
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<EstadoPedido> EstadoPedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<PedidoProducto> PedidoProductos { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_coneccionString).UseLazyLoadingProxies()
                .LogTo(s => System.Console.WriteLine(s));

        }
    }
}
