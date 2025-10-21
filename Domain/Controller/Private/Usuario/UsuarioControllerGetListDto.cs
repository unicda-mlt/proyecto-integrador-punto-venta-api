
using Domain.API;

namespace Domain.Controller.Private.Usuario
{
    public class UsuarioControllerGetListDto : PaginationQueryParams
    {
        public string? UsuarioNombre { get; set; }
        public bool? Activo { get; set; }
    }
}
