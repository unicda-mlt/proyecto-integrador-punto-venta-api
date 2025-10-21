﻿using Business.Utils;
using Data.Repositories;
using Domain.API;
using Domain.Authentication;
using Domain.Controller.Private.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Private.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuarioController(ICurrentUser _currentUser, UsuarioRepository _usuarioRepository) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<UsuarioControllerCreateOneResponse>(StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear un nuevo usuario",
            Description = "Crea un nuevo usuario. Retorna error si ya existe un usuario con el mismo nombre y que no este eliminado."
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
        [ProducesResponseType<BaseObjectResponse<UsuarioControllerGetByIdResponse>>(StatusCodes.Status200OK)]
        [SwaggerOperation(
              Summary = "Obtener un usuario por su ID.",
              Description = "Devuelve un usuario que no este eliminado."
          )]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _usuarioRepository.GetById(id);

            if (data == null || data.Eliminado == true) {
                return Ok(new BaseObjectResponse<object>
                {
                    Ok = true,
                    Data = null
                });
            }

            return Ok(new BaseObjectResponse<UsuarioControllerGetByIdResponse>
            {
                Ok = true,
                Data = new UsuarioControllerGetByIdResponse
                {
                    Id = data.Id,
                    Nombre = data.Nombre,
                    UsuarioNombre = data.UsuarioNombre,
                    Activo = data.Activo,
                    CreadoEn = data.CreadoEn,
                    ActualizadoEn = data.ActualizadoEn,
                }
            });
        }

        [HttpGet]
        [ProducesResponseType<PaginationResponse<UsuarioControllerGetListResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
              Summary = "Obtener lista de usuarios",
              Description = "Devuelve una lista de todos los usuarios."
          )]
        public async Task<IActionResult> GetList([FromQuery] UsuarioControllerGetListDto param)
        {
            try
            {
                var pagination = await _usuarioRepository.GetAll(
                    filter: (x => !x.Eliminado
                        && (param.UsuarioNombre == null || x.UsuarioNombre.StartsWith(param.UsuarioNombre))
                        && (param.Activo == null || x.Activo == param.Activo)
                    ),
                    selector: (x => new UsuarioControllerGetListResponse
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        UsuarioNombre = x.UsuarioNombre,
                        Activo = x.Activo,
                    }),
                    orderBy: x => x.CreadoEn,
                    pageArg: param.Page,
                    pageSizeArg: param.PageSize
                );

                return Ok(pagination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BadRequestResponse { BadMessage = $"Ocurrió un error interno: {ex.Message}" });
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status404NotFound)]
        [SwaggerOperation(
             Summary = "Desactiva un usuario por ID",
             Description = "Busca un usuario por su ID y establece su campo 'Activo' a 'false'."
         )]
        public async Task<IActionResult> DeactiveById(Guid id)
        {
            try
            {
                var dbUser = await _usuarioRepository.GetById(id);

                if (dbUser == null || dbUser.Activo == false || dbUser.Eliminado == true || dbUser.Id.Equals(_currentUser.UsuarioId))
                {
                    return Ok(new OkResponse());
                }

                dbUser.Activo = false;

                await _usuarioRepository.Edit(dbUser);

                return Ok(new OkResponse());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BadRequestResponse { BadMessage = $"Ocurrió un error interno: {ex.Message}" });
            }
        }
    }
}
