
namespace Domain.Models
{
    public class CajaEstadoModel : BaseEntity<short>
    {
        public required string Nombre { get; set; }

        public ICollection<Caja> Cajas { get; set; } = [];
    }

    public enum CajaEstado : short
    {
        Cerrado = 1,
        Abierto = 2
    }

    public static class CajaEstadoExtensions
    {
        public static short GetValue(this CajaEstado estado)
        {
            return (short)estado;
        }
    }
}
