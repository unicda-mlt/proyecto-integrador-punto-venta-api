
namespace Domain.Controller.Private.Caja
{
    public class CajaControllerGetByIdResponse
    {
        public required Guid Id { get; set; }
        public required short EstadoId { get; set; }
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public required bool Activo { get; set; }
        public required DateTime CreadoEn { get; set; }
        public required DateTime? ActualizadoEn { get; set; }
    }
}
