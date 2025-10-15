using Business.Utils;
using Data.Repositories;
using Domain.API;
using Domain.Controller.Private.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Private.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController(UsuarioRepository usuarioRepository) : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository = usuarioRepository;

        [HttpPost("CreateOne")]
        [ProducesResponseType<UsuarioControllerCreateOneResponse>(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear una nueva categoría de evento",
            Description = "Crea una nueva categoría de evento con el nombre proporcionado. Si ya existe una categoría con el mismo nombre, retorna el ID de la categoría existente."
        )]
        public async Task<IActionResult> CreateOne([FromBody] UsuarioControllerCreateOneDto data)
        {
            var dbUsuario = await _usuarioRepository.GetOneByFilter(x => x.UsuarioNombre.Equals(data.UsuarioNombre) && !x.Eliminado);

            if (dbUsuario != null)
            {
                return BadRequest(new BadRequestResponse {
                    BadMessage="Ya existe un usuario con el nombre de usuario enviado."
                });
            }

            var newUsuario = await _usuarioRepository.Create(new()
            {
                RolId = 1,
                Password = PasswordHasher.HashPassword(data.Password),
                Nombre = data.Nombre,
                UsuarioNombre = data.UsuarioNombre,
                Activo = data.Activo,
                Eliminado = false
            });

            return Ok(new
            {
                newUsuario.Id
            });
        }
    }
}
