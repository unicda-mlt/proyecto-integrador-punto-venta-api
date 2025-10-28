using Domain.API;

namespace Domain.Controller.Private.Caja
{
    public class CajaControllerGetListDto : PaginationQueryParams
    {
        public string? Nombre { get; set; }

        public string? Codigo { get; set; }

        public bool? Activo { get; set; }
    }
}