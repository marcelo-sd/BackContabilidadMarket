using AutoMapper;
using ContabilidaMarket.Context;
using ContabilidaMarket.Models.DTOs;
using ContabilidaMarket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ContabilidaMarket.Controllers
{
    public class PedidosProductoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PedidosProductoController> _logger;

        public PedidosProductoController(AppDbContext context, IMapper mapper,
            ILogger<PedidosProductoController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
                 
        }

        [HttpGet]
        [Route("/obtenerListaDeProductosEnPedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [Authorize]
        public async Task<IActionResult> obtenerListaDeProductosEnPedido()
        {
            try
            {
                var listaPro = await _context.PedidoProductos.ToListAsync();
                //   var listaPE= _mapper.Map<PedidosDto>(listaPe);


                if (listaPro == null || !listaPro.Any())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "lista vacia o null");
                }
                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    response = listaPro
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("/crearProductoEnPedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> crearProductoEnPedido([FromBody] PedidoProductoDto nuevoProducto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                // Verificar si el Pedido existe
                var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.IdPedido == nuevoProducto.IdPedido);
                if (!pedidoExiste) 
                { 
                    return StatusCode(StatusCodes.Status404NotFound,
                        $"El idPedido {nuevoProducto.IdPedido} no pudo ser encontrado."); 
                }

                _logger.LogInformation("Creando nuevo Producto con Pedido ID: {IdPedido}", nuevoProducto.IdPedido);

        

                var producto = _mapper.Map<PedidoProducto>(nuevoProducto);

                _context.PedidoProductos.Add(producto);

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, $"Producto creado exitosamente con el idPedido: {nuevoProducto.IdPedido}");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error durante la operación");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message, innerException = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }


        [HttpPut]
        [Route("/modificarProductoEnPedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> modificarProductoEnPedido([FromBody] PedidoProductoPutDto oPedidoProducto)

        {

            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            try
            {
                var pedidoProductoEncontrado = await _context.PedidoProductos.SingleOrDefaultAsync(p => p.Id == oPedidoProducto.Id  );
                if (pedidoProductoEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "no se encontro el PediodoProducto");
                }

                if (oPedidoProducto.IdPedido.HasValue)
                {
                    if (!_context.Pedidos.Any(u => u.IdPedido == oPedidoProducto.IdPedido))
                    {
                        return StatusCode(StatusCodes.Status404NotFound,
                            $"el id del Pedido {oPedidoProducto.IdPedido} no se encuentra en la base de datos");
                    }
                }

                if (oPedidoProducto.IdProducto.HasValue)
                {
                    if (!_context.Productos.Any(u => u.IdProductos == oPedidoProducto.IdProducto))
                    {
                        return StatusCode(StatusCodes.Status404NotFound,
                            $"el id del Producto {oPedidoProducto.IdPedido} no se encuentra en la base de datos");
                    }
                }

                pedidoProductoEncontrado.IdPedido = oPedidoProducto.IdPedido ?? pedidoProductoEncontrado.IdPedido;
                pedidoProductoEncontrado.IdProducto  = oPedidoProducto.IdProducto ?? pedidoProductoEncontrado.IdProducto;


                _context.PedidoProductos.Update(pedidoProductoEncontrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK,
                    new { message = $"se modifico correctamente el pedidoProducto: " +
                    $"{pedidoProductoEncontrado.Id}" });


            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error durante la operación");
                // Obtener detalles adicionales sobre la excepción
                var affectedEntries = ex.Entries.Select(e => 
                new { Entity = e.Entity.GetType().Name, State = e.State.ToString() }).ToList();
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { message = ex.Message,
                        innerException = ex.InnerException?.Message, 
                        affectedEntries });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }


        [HttpDelete]
        [Route("/eliminarPedidoProducto/{idPedido:int}")]
        //  [ValidateAntiForgeryToken]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EliminarPedidoProducto(int idPedido)
        {

            try
            {
                var pedidoProductoEncintrado = await _context.PedidoProductos.SingleOrDefaultAsync(p => p.Id == idPedido);
                if (pedidoProductoEncintrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"pedidoProducto: {idPedido} no encontrado en la base de datos");
                }
                _context.PedidoProductos.Remove(pedidoProductoEncintrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"se elimino el PedidoProducto: {idPedido}");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }









    }
}
