using AutoMapper;
using ContabilidaMarket.clasesUtiles;
using ContabilidaMarket.Context;
using ContabilidaMarket.interfaces01;
using ContabilidaMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContabilidaMarket.Controllers
{
    public class AltasMainController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAltasMain _altas;
        public AltasMainController(AppDbContext context, IMapper mapper, IAltasMain altas)
        {
            _context = context;
            _mapper = mapper;
            _altas = altas;
        }




        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]  // No autorizado
        [ProducesResponseType(403)] //no permitido

        [Route("/altasMain")]
        [Authorize(Policy = "RequireAdminRole")] 
        public async Task<IActionResult> AltasMain([FromBody] ModeloVenta nuevaVenta)
        {

            string res = await _altas.RegistrarVentas(nuevaVenta);

            if (res != "ok")
            {
                return StatusCode(StatusCodes.Status400BadRequest, res);
            }

            return StatusCode(StatusCodes.Status200OK, res);

        }



    }
}
