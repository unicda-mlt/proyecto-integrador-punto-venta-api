

using Domain.INVENTARIO_API;

namespace Business.Services.INVENTARIO_API
{
    public class INVENTARIOEditorialService(INVENTARIOApiService api)
    {
        private readonly INVENTARIOApiService _api = api;

        public Task<GetEditorialByIdResponse?> GetByIdAsync(
            short Id,
            CancellationToken ct = default
        ) => _api.GetAsync<GetEditorialByIdResponse?>($"/internal/editorial/{Id}", ct);
    }
}
