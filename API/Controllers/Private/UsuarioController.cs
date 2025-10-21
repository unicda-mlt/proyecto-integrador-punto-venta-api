using Business.Services;
using Business.Utils;
using Data.Repositories;
using Domain.API;
using Domain.API.Interfaces;
using Domain.Controller.Private.Usuario;
using Domain.Models;
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

        [HttpGet]
        [ProducesResponseType<IGetAllReturn<Usuario>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
              Summary = "Obtener lista de usuarios",
              Description = "Devuelve una lista de todos los usuarios."
          )]
        public async Task<IActionResult> ObtenerListaUsuarios([FromQuery] int? page, [FromQuery] byte? pageSize)
        {
            try
            {
                var resultado = await _usuarioRepository.GetAll(page, pageSize);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BadRequestResponse { BadMessage = $"Ocurrió un error interno: {ex.Message}" });
            }
        }


        [HttpPost("desactivar/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
             Summary = "Desactiva un usuario por ID",
             Description = "Busca un usuario por su ID y establece su campo 'Activo' a 'false'."
         )]
        public async Task<IActionResult> DesactivarUsuario(Guid id)
        {
            try
            {
                var usuario = await _usuarioRepository.GetById(id);
                if (usuario == null)
                {
                    return NotFound(new BadRequestResponse { BadMessage = "No se encontró un usuario con el ID proporcionado." });
                }

                if (usuario.Activo == false)
                {
                    return BadRequest(new BadRequestResponse { BadMessage = "El usuario ya se encuentra desactivado." });
                }

                usuario.Activo = false;

                await _usuarioRepository.Edit(usuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BadRequestResponse { BadMessage = $"Ocurrió un error interno: {ex.Message}" });
            }
        }
    }
}
