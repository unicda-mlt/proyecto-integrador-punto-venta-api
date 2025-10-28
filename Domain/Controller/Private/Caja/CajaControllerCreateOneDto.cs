
namespace Domain.Controller.Private.Caja
{
    public class CajaControllerCreateOneDto
    {
        public required string Codigo { get; set; }
        public required string Nombre { get; set; }
        public bool Activo { get; set; } = false;
    }
}
