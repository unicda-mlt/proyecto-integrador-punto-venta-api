
namespace Domain.INVENTARIO_API
{
    public class GetCategoriaEventoByIdResponse
    {
        public required DataObj Data { get; set; }

        public class DataObj
        {
            public required short CategoriaEventoId {  get; set; }
            public required String Nombre { get; set; }
        }
    }
}
