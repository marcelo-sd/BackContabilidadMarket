using AutoMapper;
using ContabilidaMarket.Context;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ContabilidaMarket.Controllers
{
    public class RolController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RolController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: RolController
        [HttpGet("ObtenerRoles")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> ObtenerRoles()
        {
            try
            {
                var roles = await _context.Rols.ToListAsync();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    Message = "esta es la lista ",
                    Response = roles

                });
            }
            catch (Exception ex) { 
            return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
          

        }




        [HttpPost("SubirRol")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
     
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<ActionResult> SubirRol([FromBody] RolDto rol)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            try
            {
                var objeto = _mapper.Map<Rol>(rol);
                _context.Rols.Add(objeto);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, "el rol se subio exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpPut]
        [Route("/modificarRol")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]  // No autorizado
      
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> ModificarRol([FromBody] RolPutDto Orol)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }
            try
            {
                Rol?  rolEncontrado = await _context.Rols.FindAsync(Orol.IdRol);
                if(rolEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"el IdRol {Orol.IdRol} no fue encontrado");
                }
                rolEncontrado.RolNombre = Orol.RolNombre ?? rolEncontrado.RolNombre;
                _context.Rols.Update(rolEncontrado);

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new
                {
                    message = "Ok",
                    response = rolEncontrado

                });


            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    innerError = ex.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





        [HttpDelete]
        [Route("/eliminarRol/{idRol:int}")]
        //  [ValidateAntiForgeryToken]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> EliminaRol(int idRol)
        {

            try
            {
                var rolEncontrado = await _context.Rols.SingleOrDefaultAsync(p => p.IdRol == idRol);
                if (rolEncontrado == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, $"Rol: {idRol} no encontrado en la base de datos");
                }
                _context.Rols.Remove(rolEncontrado);

                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, $"se elimino el Rol: {rolEncontrado.RolNombre}");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }

    }
}
