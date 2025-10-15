
namespace Domain.Controller.Private.Auth
{
    public class GenerateUserTokenDto
    {
        public required string UsuarioNombre { get; set; }
        public required string Password { get; set; }
    }
}
