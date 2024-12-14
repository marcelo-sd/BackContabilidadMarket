using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContabilidaMarket.Context;
using ContabilidaMarket.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ContabilidaMarket.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using WorkerService0001;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ContabilidaMarket.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IColaDeTareasEnSEgundoPlano _taskQueue;
        //private readonly IDistributedCache _distributedCache;

        public UsuariosController(AppDbContext context, IMapper mapper
            , IColaDeTareasEnSEgundoPlano taskQueue
            )
        {
            _context = context;
            _mapper = mapper;
            _taskQueue = taskQueue;
            //_distributedCache = distributedCache;
        }



        [HttpGet]
        [Route("ListaUsuarios")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> ListaUsuarios()
        {
            //  string? respuesta = await _distributedCache.GetStringAsync("MiClave");
            // if (respuesta == null)
            //  {
            try
            {
                var listaUsuarios = await _context.Usuarios.ToListAsync();
                //respuesta = JsonConvert.SerializeObject(listaUsuarios);
                //  await _distributedCache.SetStringAsync("MiClave", respuesta, new DistributedCacheEntryOptions
                //  {
                //     AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                //aqui configuro  el tiempo que va a durar la respuesta en el cache
                // });

                return StatusCode(StatusCodes.Status200OK, new { Response = listaUsuarios });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            //  }
            //   else
            // {
            //   var listausuarios =JsonConvert.DeserializeObject<List<Usuario>>(respuesta);

            //  return StatusCode(StatusCodes.Status200OK, new { Response = listausuarios });
            // }
        }




        [HttpGet("ListaUsuariosDto")]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize]
        [Authorize(Policy = "RequireAdminRole")]

        public async Task<IActionResult> ListaUsuariosDto()
        {
            try
            {
                var listaUsuarios = await _context.Usuarios.ToListAsync();
                var listaUsuariosDto = _mapper.Map<List<UsuarioDto>>(listaUsuarios);
                // var listaUsuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();

                return Ok(new { Response = listaUsuariosDto });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("TestLazyLoading")]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> TestLazyLoading()
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == 1);
                var usuarioDto = _mapper.Map<UsuarioDto>(usuario);
                //de esta manera EF no va a hacer la consulta a la tabla rol ya que lazy
                //loading detecta que no es nesesario la  carga de la propiedad rol

                // Acceder a la propiedad de navegación para disparar lazy loading


                return Ok(new { Usuario = usuarioDto });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(403)]
        [ProducesResponseType(401)]
        [HttpGet]
        [Route("obtenerUsuario/{IdUsuario:int}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Obtenerusuario(int IdUsuario)
        {


            Usuario oUsuario = await _context.Usuarios.FindAsync(IdUsuario);
            if (oUsuario == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "usuario no encontrado");
            }
            try
            {
                var usuarioDto = _mapper.Map<Usuario>(oUsuario);
                return StatusCode(StatusCodes.Status200OK, new { messge = "Ok", Response = usuarioDto });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }








        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [HttpPost("CargarUsuario")]
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> CargarUsuario([FromBody] UsuarioDto usuarioDto)
        {
            //es para verificar los requerimentos
            if (ModelState.IsValid)
            {
                try
                {

                    var usuario = _mapper.Map<Usuario>(usuarioDto);
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status200OK, "El usuario se cargo correctamente");
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }

            }
            else
            {
                return BadRequest(ModelState);
            }

        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización
        [Route("EditarUsuario")]
        public async Task<IActionResult> EditarUsuario([FromBody] UsuarioPutDto objeto)
        {

            if (objeto == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Objeto no puede ser null");
            }

            _taskQueue.ElementoDTrabajoEnSEgundoPlano(async token =>
            {
                ///aqui deberia ir mi logica 

            });
            Usuario oUsuarioEncontrado = await _context.Usuarios.FindAsync(objeto.IdUsuario);
            if (oUsuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "No se encontro el usuario");
            }
            try
            {
                oUsuarioEncontrado.Nombre = objeto.Nombre is null ? oUsuarioEncontrado.Nombre : objeto.Nombre;
                oUsuarioEncontrado.Apellido = objeto.Apellido is null ? oUsuarioEncontrado.Apellido : objeto.Apellido;
                oUsuarioEncontrado.Password = objeto.Password is null ? oUsuarioEncontrado.Password : objeto.Password;
                oUsuarioEncontrado.Telefono = objeto.Telefono is null ? oUsuarioEncontrado.Telefono : objeto.Telefono;
                oUsuarioEncontrado.Rol_id = objeto.Rol_id ?? oUsuarioEncontrado.Rol_id;
                _context.Usuarios.Update(oUsuarioEncontrado);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK,
                    new { message = $"Se ha actualizado el Usuario  {oUsuarioEncontrado.Nombre}" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });

            }
        }


        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido
        [Route("EliminarUsuario/{Id:int}")]
        [Authorize(Policy = "RequireAdminRole")] // Aplicar la política de autorización

        public async Task<IActionResult> EliminarUsuario(int Id)
        {

            Usuario oUsuarioEncontrado = await _context.Usuarios.FindAsync(Id);
            if (oUsuarioEncontrado == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Usuario no encontrado");
            }
            try
            {
                _context.Usuarios.Remove(oUsuarioEncontrado);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = $"Se ha eliminado el Usuario: =={oUsuarioEncontrado.Nombre}== exitosamente!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { ex.Message });

            }
        }













    }
}
