

using Domain.INVENTARIO_API;

namespace Business.Services.INVENTARIO_API
{
    public class INVENTARIOEventoService(INVENTARIOApiService api)
    {
        private readonly INVENTARIOApiService _api = api;

        public Task<GetEventoByIdResponse?> GetByIdAsync(
            int Id,
            CancellationToken ct = default
        ) => _api.GetAsync<GetEventoByIdResponse?>($"/internal/evento/{Id}", ct);

        public Task<GetEventoResponse?> GetAllAsync(
            DateTime? FromDate = default,
            DateTime? ToDate = default,
            CancellationToken ct = default
        )
        {
            var queryParams = new Dictionary<string, string>();

            if (FromDate.HasValue)
            {
                queryParams["fechaInicio"] = FromDate.Value.ToString("yyyy-MM-dd hh:mm:ss");
            }

            if (ToDate.HasValue)
            {
                queryParams["fechaFin"] = ToDate.Value.ToString("yyyy-MM-dd hh:mm:ss");
            }

            if ((FromDate.HasValue && !ToDate.HasValue) || (ToDate.HasValue && !FromDate.HasValue))
            {
                queryParams = [];
            }

            return _api.GetAsync<GetEventoResponse?>("/internal/evento/", queryParams, ct);
        }
    }
}
