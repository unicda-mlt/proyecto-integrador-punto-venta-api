
using Domain.API;

namespace Domain.Controller.Private.Caja
{
    public class CajaControllerGetListDto : PaginationQueryParams
    {
        public short? EstadoId { get; set; }
        public string? Codigo { get; set; }
        public bool? Activo { get; set; }
    }
}
