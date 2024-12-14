using AutoMapper;
using ContabilidaMarket.Context;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace ContabilidaMarket.Controllers
{
    public class PedidoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(AppDbContext context, IMapper mapper, ILogger<PedidoController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("/obtenerListaDePedidos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [Authorize]
        public async Task<IActionResult> ObtenerListaDePedidos()
        {
            try
            {
                var listaPe = await _context.Pedidos.ToListAsync();
             //   var listaPE= _mapper.Map<PedidosDto>(listaPe);


                if (listaPe == null || !listaPe.Any())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "lista vacia o null");
                }
                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    response = listaPe
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message= ex.Message
                });
            }
        }
        [HttpPost]
        [Route("/crearPedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]

        public async Task<IActionResult> CrearProducto([FromBody] PedidoPostDto nuevoPedido)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                _logger.LogInformation("Creando nuevo pedido con usuario ID: {IdUsuario}", nuevoPedido.idUsuario);

                // Verificar si el Estado existe
                var estadoExiste = await _context.EstadoPedidos.AnyAsync(e => e.IdEstado == nuevoPedido.EstadoPedidoId);
                if (!estadoExiste)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "El estado proporcionado no existe.");
                }

                var pedido = _mapper.Map<Pedido>(nuevoPedido);
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "Pedido creado exitosamente.");
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
        [Route("/modificarPedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> ModificarPedido([FromBody] PedidoPutDto oPedido)

        {

            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            try
            {
                Pedido pedidoEncontrado = await _context.Pedidos.SingleOrDefaultAsync(p => p.IdPedido == oPedido.IdPedido);
                if (pedidoEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "no se encontro el Pediodo");
                }
                if (oPedido.idUsuario.HasValue)
                {
                    if (!_context.Usuarios.Any(u => u.IdUsuario == oPedido.idUsuario))
                    {
                        return StatusCode(StatusCodes.Status404NotFound,
                            $"el id del Usuario {oPedido.idUsuario} no se encuentra en la base de datos");
                    }
                }
            
                pedidoEncontrado.idUsuario = oPedido.idUsuario ?? pedidoEncontrado.idUsuario;
                pedidoEncontrado.EstadoPedidoId = oPedido.EstadoPedidoId ?? pedidoEncontrado.EstadoPedidoId;
         

                _context.Pedidos.Update(pedidoEncontrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, 
                    new { message = $"se modifico correctamente el pedido: {pedidoEncontrado.IdPedido}" });


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }


        [HttpDelete]
        [Route("/elinimarPedido/{idPedido:int}")]
        //  [ValidateAntiForgeryToken]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EliminarPedido(int idPedido)
        {
         
            try
            {
                var pedidoEncontrado = await _context.Pedidos.SingleOrDefaultAsync(p => p.IdPedido == idPedido);
                if (pedidoEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"pedido: {idPedido} no encontrado");
                }
                _context.Pedidos.Remove(pedidoEncontrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"se elimino el Pedido: {idPedido}");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }




        [HttpPatch("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PatchUsuario(int id, [FromBody] PedidoPostDto pedidoParcial)
        {
            var pedidoEncontrado = await _context.Pedidos.FindAsync(id);
            if (pedidoEncontrado == null)
            {
                return NotFound("Usuario no encontrado");
            }

            if (pedidoParcial.idUsuario != null)
            {
                pedidoEncontrado.idUsuario = pedidoParcial.idUsuario;
            }
            else
            {
                pedidoEncontrado.idUsuario = pedidoEncontrado.idUsuario;
            }

            if (pedidoParcial.EstadoPedidoId != null)
            {
                pedidoEncontrado.EstadoPedidoId = pedidoParcial.EstadoPedidoId;
            }
            else
            {
                pedidoEncontrado.EstadoPedidoId = pedidoEncontrado.EstadoPedidoId;
            }

            await _context.SaveChangesAsync();

            return Ok("Usuario actualizado");
        }







    }
}
