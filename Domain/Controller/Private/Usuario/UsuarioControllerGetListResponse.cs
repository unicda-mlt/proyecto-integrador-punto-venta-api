
using Domain.API;

namespace Domain.Controller.Private.Usuario
{
    public class UsuarioControllerGetListResponse
    {
        public required Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string UsuarioNombre { get; set; }
        public required bool Activo { get; set; }
    }
}
