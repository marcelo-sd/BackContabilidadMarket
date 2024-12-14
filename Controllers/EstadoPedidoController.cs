using AutoMapper;
using ContabilidaMarket.Context;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;

namespace ContabilidaMarket.Controllers
{
    public class EstadoPedidoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public EstadoPedidoController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("/obtenerEstadoDePedido")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]  // No autorizado
     
        [Authorize]
        public async Task<IActionResult> ObtenerEstadoDePedido()
        {
            try
            {
                var Response = await _context.EstadoPedidos.ToListAsync();
                if (Response == null || !Response.Any())
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new
                    {
                        message = "la lista esta vacio o es null"
                    });

                }

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    response = Response
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }


        [HttpPost]
        [Route("/cargarEstadoDePedio")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> CargarEstadoDePedido([FromBody] EstadoPedidoDto newEstado)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            }
            try
            {
                var NuevoEstado = _mapper.Map<EstadoPedido>(newEstado);
                _context.Add(NuevoEstado);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "ok");


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("/modificarNombreEstado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> ModificarNombreEstado([FromBody] EstadoPedidoDto Oestado)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);

            }
            try
            {
                //SingleOrDefaultAsync para busqueda mas especificas
                // var entity = await _context.Entities.SingleOrDefaultAsync(e => e.Property == value);

                var EstadoEncontrado = await _context.EstadoPedidos.FindAsync(Oestado.IdEstado);
                if (EstadoEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "estado no encontrado");
                }
                EstadoEncontrado.Estado = Oestado.Estado ?? EstadoEncontrado.Estado;
                _context.EstadoPedidos.Update(EstadoEncontrado);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"se acualizo el usuario {EstadoEncontrado.Estado}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }



        [HttpDelete]
        [Route("/borrarEstadoPedido/{id:int}\"")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> BorrarEstadoPedido(int id)
        {
            if (id <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "id debe ser mayor a 0");
            }
            var estadoEncontrado = await _context.EstadoPedidos.FindAsync(id);
            if (estadoEncontrado == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "no se encontro el estado buscado");
            }


            try
            {

                _context.EstadoPedidos.Remove(estadoEncontrado);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"el estado {estadoEncontrado.Estado} se elimino correctamente");


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Hubo un problema al eliminar el estado", details = ex.Message });
            }
        }





    }
}
