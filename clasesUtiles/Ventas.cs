using AutoMapper;
using ContabilidaMarket.Context;
using ContabilidaMarket.interfaces01;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContabilidaMarket.clasesUtiles
{
    public class Ventas : IAltasMain
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public Ventas(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }


       //aqui lo registro al pedido
        public async Task<string> AltaPedidido(int idUsuarioP, int EstadoPedidoIdP,List<PedidoProductoDto> ListaDeproductos )
        {
            using var transaction= _context.Database.BeginTransaction();
            try
            {
                PedidoPostDto nuevoPedido = new PedidoPostDto
                    {
                    idUsuario = idUsuarioP,
                    EstadoPedidoId = EstadoPedidoIdP
                };

                var pedido = _mapper.Map<Pedido>(nuevoPedido);
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                int idPedidoActual = await _context.Pedidos
          .Where(p => p.idUsuario == idUsuarioP)
          .OrderByDescending(p => p.IdPedido).Select(p => p.IdPedido)
          .FirstOrDefaultAsync();

                var productosDuplicados = ListaDeproductos.GroupBy(p => p.IdProducto)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key).ToList();

                if(productosDuplicados.Any())
                {
                    return "la lista de productos no puede tener 2 productos con el mismo idProducto";
                }

                foreach (var producto in ListaDeproductos)
                {
                    // Verificar si el Pedido existe
                    producto.IdPedido = idPedidoActual;

                    var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.IdPedido == producto.IdPedido);
                    if (!pedidoExiste)
                    {
                        return $"El idPedido {producto.IdPedido} no pudo ser encontrado.";
                    }
                    
                    if(producto.Cantidad == null ||  producto.Cantidad == 0)
                    {
                        return $"el Producto debe contener la cantidad , debe ser diferente a 0 ";
                    }

                    PedidoProducto ProductoNuevo = _mapper.Map<PedidoProducto>(producto);
                    _context.PedidoProductos.Add(ProductoNuevo);

                    //aqui manejo el stock de cada producto 
                    Producto productoStokEncontrado = await _context.Productos.FindAsync(producto.IdProducto);

                    ///tengo que verificar si el estok es suficiente
                    if(productoStokEncontrado.Stock==0 ||producto.Cantidad > productoStokEncontrado.Stock)
                    {
                        return $"no hay stock o la cantidad exede el stok disponible de producto  con el id: {producto.IdProducto}";
                    }

                    productoStokEncontrado.Stock=productoStokEncontrado.Stock - producto.Cantidad;

                    _context.Productos.Update(productoStokEncontrado);



                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return "ok";


            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                return ex.InnerException?.Message ?? "Error de actualización de base de datos";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();  
                string message = ex.Message;
                return message;
            }
        }
        //registro de productos del pedido







       public async Task<string> RegistrarVentas(ModeloVenta newVenta)
        {
            string RegistroPedido= await AltaPedidido(newVenta.idUsuario, newVenta.EstadoPedidoId,newVenta.ListaProductosEnElPedido);

            if (RegistroPedido != "ok") 
            {
                return RegistroPedido;
            }
            return "ok";
        }
    }
}
