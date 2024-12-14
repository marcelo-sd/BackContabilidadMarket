using ContabilidaMarket.Context;
using ContabilidaMarket.Models;
using ContabilidaMarket.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContabilidaMarket.Jwt;
using ContabilidaMarket.clasesUtiles;



namespace ContabilidaMarket.Controllers
{

    [Route("/auth")]
    [ApiController]
    public class InicioSecionController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public InicioSecionController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpPost("ObtenerToken")]
        public async Task<ActionResult> ObtenerToken([FromBody] UsuarioInicioDeSecion optData)
        {            
            string nameUser = optData.nombre;
            Usuario usuarioEncontrado = await _context.Usuarios
                .Where(u => u.Nombre == nameUser).FirstOrDefaultAsync();
            if (usuarioEncontrado is null)
            {
                return StatusCode(StatusCodes.Status404NotFound,"usuario no encontrado");
            }
            var jwt = _configuration.GetSection("Jwt").Get<Jwtcls>();
            jwt.Subject =usuarioEncontrado.IdUsuario.ToString();           
            var claims = new[]
            {
                     new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.UtcNow)
                        .ToUnixTimeSeconds().ToString()),
                     new Claim("name", usuarioEncontrado.Nombre.ToString()),
                     new Claim(ClaimTypes.Role,usuarioEncontrado.Rol_id.ToString())
            };
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
            var iniciosecion = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
              audience: jwt.Audience,
               claims: claims,
              expires: DateTime.UtcNow.AddMinutes(30),  // Usar DateTimeOffset.UtcNow para formato Unix
               signingCredentials: iniciosecion
                );
            var result = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Message = "ok", Response = result });
        }



    }
}
