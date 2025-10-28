using Data.Repositories;
using Domain.API;
using Domain.Controller.Private.Caja;
using Domain.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Private.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")] 
    [ApiController]
    public class CajaController(CajaRepository _cajaRepository) : ControllerBase
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
                EstadoId = CajaEstado.Cerrado.GetValue(),
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

        [HttpPost]
        [ProducesResponseType<OkResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType<BadRequestResponse>(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Editar una caja",
            Description = "Edita los datos de una caja. La caja no debe de estar eliminada para poder ser editada."
        )]
        public async Task<IActionResult> EditOne(Guid id, [FromBody] CajaControllerEditOneDto data)
        {
            var dbCaja = await _cajaRepository.GetOneByFilter(x => x.Id.Equals(id));

            if (dbCaja == null || dbCaja.Eliminado == true)
            {
                return Ok(new OkResponse());
            }
            else if (dbCaja.EstadoId == CajaEstado.Abierto.GetValue())
            {
                return BadRequest(new BadRequestResponse
                {
                    BadMessage = "La caja se encuentra abierta."
                });
            }

                Caja? dbCajaConCodigo = data.Codigo == null ? null : await _cajaRepository.GetOneByFilter(x => x.Codigo.Equals(data.Codigo) && !x.Id.Equals(id));

            if (dbCajaConCodigo != null)
            {
                return BadRequest(new BadRequestResponse
                {
                    BadMessage = "Ya existe una caja con el código enviado."
                });
            }

            dbCaja.Codigo = data.Codigo ?? dbCaja.Codigo;
            dbCaja.Nombre = data.Nombre ?? dbCaja.Nombre;
            dbCaja.Activo = data.Activo ?? dbCaja.Activo;

            await _cajaRepository.Edit(dbCaja);

            return Ok(new OkResponse());
        }

        [HttpGet]
        [ProducesResponseType<BaseObjectResponse<CajaControllerGetByIdResponse>>(StatusCodes.Status200OK)]
        [SwaggerOperation(
              Summary = "Obtener una caja por su ID.",
              Description = "Devuelve una caja que no este eliminado."
          )]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await _cajaRepository.GetById(id);

            if (data == null || data.Eliminado == true)
            {
                return Ok(new BaseObjectResponse<object>
                {
                    Ok = true,
                    Data = null
                });
            }

            return Ok(new BaseObjectResponse<CajaControllerGetByIdResponse>
            {
                Ok = true,
                Data = new CajaControllerGetByIdResponse
                {
                    Id = data.Id,
                    EstadoId = data.EstadoId,
                    Codigo = data.Codigo,
                    Nombre = data.Nombre,
                    Activo = data.Activo,
                    CreadoEn = data.CreadoEn,
                    ActualizadoEn = data.ActualizadoEn,
                }
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
            var pagination = await _cajaRepository.GetAll(
                filter: (x => !x.Eliminado
                    && (param.EstadoId == null || x.EstadoId.Equals(param.EstadoId))
                    && (param.Codigo == null || x.Codigo.StartsWith(param.Codigo))
                    && (param.Activo == null || x.Activo == param.Activo)
                ),
                ["CajaEstado"],
                selector: (x => new CajaControllerGetListResponse
                {
                    Id = x.Id,
                    EstadoId = x.EstadoId,
                    Nombre = x.Nombre,
                    Codigo = x.Codigo,
                    Estado = x.CajaEstado.Nombre,
                    Activo = x.Activo,
                }),
                orderBy: x => x.CreadoEn,
                pageArg: param.Page,
                pageSizeArg: param.PageSize
            );

            return Ok(pagination);
        }

        [HttpPost]
        [ProducesResponseType<OkResponse>(StatusCodes.Status200OK)]
        [SwaggerOperation(
             Summary = "Desactiva una caja por ID",
             Description = "Busca una caja por su ID y establece su campo 'Activo' a 'false'."
         )]
        public async Task<IActionResult> DeactiveById(Guid id)
        {
            var dbCaja = await _cajaRepository.GetById(id);

            if (dbCaja == null || dbCaja.Activo == false || dbCaja.Eliminado == true)
            {
                return Ok(new OkResponse());
            }

            if (dbCaja.EstadoId == CajaEstado.Abierto.GetValue())
            {
                return BadRequest(new BadRequestResponse
                {
                    BadMessage = "La caja se encuentra abierta."
                });
            }

            dbCaja.Activo = false;

            await _cajaRepository.Edit(dbCaja);

            return Ok(new OkResponse());
        }
    }
}