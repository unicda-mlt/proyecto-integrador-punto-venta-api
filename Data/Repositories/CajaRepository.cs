using Domain.Models;

namespace Data.Repositories
{
    public class CajaRepository(AppDbContext context) : GenericRepository<Guid, Caja>(context) { }
}
