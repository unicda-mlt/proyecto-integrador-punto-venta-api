
namespace Domain.INVENTARIO_API
{
    public class GetLibroByIdResponse
    {
        public required DataObj Data { get; set; }

        public class DataObj
        {
            public required int LibroId {  get; set; }
            public required String Titulo { get; set; }
            public String? Sipnosis { get; set; }
            public short? YearPublicacion { get; set; }
            public String? ImagenUrl { get; set; }
            public required EditorialObj Editorial { get; set; }
        }

        public class EditorialObj
        {
            public required short EditorialId { get; set; }
        }
    }
}
