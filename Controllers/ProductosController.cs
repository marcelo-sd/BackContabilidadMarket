using AutoMapper;
using ContabilidaMarket.Context;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace ContabilidaMarket.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("/obtenerListaDeProductos")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [Authorize]
        public async Task<IActionResult> ObtenerListaDeproductos()
        {
            try
            {
                List<Producto> listaPro = await _context.Productos.ToListAsync();
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }



        [HttpGet]
        [Route("obtenerProductoPorCodigo/{codigoBarra}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> ObtenerProductoPorCodigo(string codigoBarra)
        {
            var producto = await _context.Productos
                .SingleOrDefaultAsync(p => p.CodigoBarra == codigoBarra);

            try
            {
                if (producto == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Producto no encontrado.");
                }

                return StatusCode(StatusCodes.Status200OK, producto);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }

        }



        [HttpPost]
        [Route("/crearProducto")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> CrearProducto([FromBody] ProductoDto nuevoProducto)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            // Verificar si el código de barras ya existe
            var productoExistente = await _context.Productos
                .SingleOrDefaultAsync(p => p.CodigoBarra == nuevoProducto.CodigoBarra);


            if (productoExistente != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "El código de barras ya está en uso.");
            }

            try
            {
                var producto = _mapper.Map<Producto>(nuevoProducto);
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "Producto creado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        [Route("/modificarProducto")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> ModificarProducto([FromBody] ProductoPutDto oProducto)

        {

            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            try
            {
                Producto productoEncontrado = await _context.Productos.SingleOrDefaultAsync(p => p.CodigoBarra == oProducto.CodigoBarra);
                if (productoEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "no se encontro en producto");
                }
                productoEncontrado.Nombre = oProducto.Nombre ?? productoEncontrado.Nombre;
                productoEncontrado.CodigoBarra = oProducto.CodigoBarra ?? productoEncontrado.CodigoBarra;
                productoEncontrado.Marca = oProducto.Marca ?? productoEncontrado.Marca;
                productoEncontrado.Precioo=oProducto.Precioo ?? productoEncontrado.Precioo;
           //     productoEncontrado.Stock = oProducto.Stock >= 0 ? oProducto.Stock : productoEncontrado.Stock;
                productoEncontrado.Stock = oProducto.Stock.HasValue && oProducto.Stock >= 0 ? oProducto.Stock.Value : productoEncontrado.Stock; 
                
                
                _context.Productos.Update(productoEncontrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = $"se modifico correctamente {productoEncontrado.CodigoBarra}" });


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }


        [HttpDelete]
        [Route("/elinimarProducto/{codigoBarra}")]
        //  [ValidateAntiForgeryToken]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EliminarProducto(string codigoBarra)
        {
            if (!Regex.IsMatch(codigoBarra, @"^\d+$"))
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                "El código solo debe contener números.");
            }
            try
            {
                var productoEncontrado = await _context.Productos.SingleOrDefaultAsync(p => p.CodigoBarra == codigoBarra);
                if (productoEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"producto {codigoBarra} no encontrado");
                }
                _context.Productos.Remove(productoEncontrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"se elimino el producto {codigoBarra}");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }


    }
}
