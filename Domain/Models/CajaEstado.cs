
namespace Domain.Models
{
    public class CajaEstadoModel : BaseEntity<short>
    {
        public required string Nombre { get; set; }

        public ICollection<Caja> Cajas { get; set; } = [];
    }

    public enum CajaEstado
    {
        Cerrado = 1,
        Abierto = 2
    }
}
