
namespace Domain.Models
{
    public class Rol: BaseEntity<short>
    {
        public string Nombre { get; set; } = default!;
        
        public ICollection<Usuario> Usuarios { get; set; } = [];
    }
}
