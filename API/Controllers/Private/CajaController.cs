using Data.Repositories;
using Domain.API;
using Domain.Authentication; 
using Domain.Controller.Private.Caja;
using Domain.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;

namespace API.Private.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")] 
    [ApiController]
    public class CajaController(ICurrentUser _currentUser, CajaRepository _cajaRepository) : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType<CajaControllerCreateOneResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Crear una nueva caja",
            Description = "Crea una nueva caja. Retorna error si ya existe una caja con el mismo código y que no este eliminada."
        )]
        public async Task<IActionResult> CreateOne([FromBody] CajaControllerCreateOneDto data)
        {
            var dbCaja = await _cajaRepository.GetOneByFilter(x => x.Codigo.Equals(data.Codigo) && !x.Eliminado);

            if (dbCaja != null)
            {
                return BadRequest(new BadRequestResponse
                {
                    BadMessage = "Ya existe una caja con el código enviado."
                });
            }

            var newCaja = await _cajaRepository.Create(new()
            {
                EstadoId = data.EstadoId,
                Codigo = data.Codigo,
                Nombre = data.Nombre,
                Activo = data.Activo,
                Eliminado = false
            });

            return Ok(new
            {
                newCaja.Id
            });
        }

        [HttpGet]
        [ProducesResponseType<PaginationResponse<CajaControllerGetListResponse>>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
              Summary = "Obtener lista de cajas",
              Description = "Devuelve una lista de todas las cajas con filtros y paginación."
          )]
        public async Task<IActionResult> GetList([FromQuery] CajaControllerGetListDto param)
        {
            try
            {
                var pagination = await _cajaRepository.GetAll(
                    filter: (x => !x.Eliminado
                        && (param.Nombre == null || x.Nombre.Contains(param.Nombre))
                        && (param.Codigo == null || x.Codigo.Contains(param.Codigo))
                        && (param.Activo == null || x.Activo == param.Activo)
                    ),
                    selector: (x => new CajaControllerGetListResponse
                    {
                        Id = x.Id,
                        Nombre = x.Nombre,
                        Codigo = x.Codigo,
                        EstadoId = x.EstadoId,
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
        [ProducesResponseType<OkResponse>(StatusCodes.Status200OK)]
        [SwaggerOperation(
             Summary = "Desactiva una caja por ID",
             Description = "Busca una caja por su ID y establece su campo 'Activo' a 'false'."
         )]
        public async Task<IActionResult> DeactiveById(Guid id)
        {
            try
            {
                var dbCaja = await _cajaRepository.GetById(id);

                // Validamos que exista, que no esté eliminada y que esté activa
                if (dbCaja == null || dbCaja.Activo == false || dbCaja.Eliminado == true)
                {
                    // No hacemos nada y devolvemos OK, igual que en UsuarioController
                    return Ok(new OkResponse());
                }

                dbCaja.Activo = false;

                await _cajaRepository.Edit(dbCaja);

                return Ok(new OkResponse());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BadRequestResponse { BadMessage = $"Ocurrió un error interno: {ex.Message}" });
            }
        }
    }
}