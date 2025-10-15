
namespace Domain.INVENTARIO_API
{
    public class GetEventoByIdResponse
    {
        public required DataObj Data { get; set; }

        public class DataObj
        {
            public required int EventoId {  get; set; }
            public required String Nombre { get; set; }
            public required String Descripcion { get; set; }
            public required String FechaInicio { get; set; }
            public required String FechaFin { get; set; }
            public required CategoriaEventoObj CategoriaEvento { get; set; }
        }

        public class CategoriaEventoObj
        {
            public required short CategoriaEventoId { get; set; }
        }
    }
}
