
namespace Domain.Controller.Private.Usuario
{
    public class UsuarioControllerCreateOneDto
    {
        public required string Nombre { get; set; }
        public required string UsuarioNombre { get; set; }
        public required string Password { get; set; }
        public bool Activo { get; set; } = false;
    }
}
