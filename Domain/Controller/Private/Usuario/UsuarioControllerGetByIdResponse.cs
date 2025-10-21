
namespace Domain.Controller.Private.Usuario
{
    public class UsuarioControllerGetByIdResponse
    {
        public required Guid Id { get; set; }
        public required string Nombre { get; set; }
        public required string UsuarioNombre { get; set; }
        public bool Activo { get; set; } = false;
        public required DateTime CreadoEn { get; set; }
        public required DateTime? ActualizadoEn { get; set; }
    }
}
