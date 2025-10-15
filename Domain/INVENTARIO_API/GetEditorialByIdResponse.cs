
namespace Domain.INVENTARIO_API
{
    public class GetEditorialByIdResponse
    {
        public required DataObj Data { get; set; }

        public class DataObj
        {
            public required short EditorialId {  get; set; }
            public required String Nombre { get; set; }
        }
    }
}
