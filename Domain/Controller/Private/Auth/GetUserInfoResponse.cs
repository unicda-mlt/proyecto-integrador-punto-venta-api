
namespace Domain.Controller.Private.Auth
{
    public class GetUserInfoResponse
    {
        public required string Nombre { get; set; }
        public required string UsuarioNombre { get; set; }
        public required RolUsuario Rol { get; set; }

        public class RolUsuario
        {
            public required short Id { get; set; }
            public required string Nombre { get; set; }
        }
    }
}
