using Domain.INVENTARIO_API;

namespace Business.Services.INVENTARIO_API
{
    public class INVENTARIOLibroService(INVENTARIOApiService api)
    {
        private readonly INVENTARIOApiService _api = api;

        public Task<GetLibroByIdResponse?> GetByIdAsync(
            int Id,
            CancellationToken ct = default
        ) => _api.GetAsync<GetLibroByIdResponse?>($"/internal/libro/{Id}", ct);

        public Task<GetLibroAllResponse?> GetAllAsync(
            String? Title = default,
            CancellationToken ct = default
        )
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(Title))
            {
                queryParams["titulo"] = Title;
            }

            return _api.GetAsync<GetLibroAllResponse?>("/internal/libro/", queryParams, ct);
        }

        public Task<INVENTARIOApiService.FilePayload> DownloadPDFAsync(
            int Id,
            CancellationToken ct = default
        ) => _api.DownloadFileAsync($"/internal/libro/{Id}/descargar", null, ct);
    }
}
