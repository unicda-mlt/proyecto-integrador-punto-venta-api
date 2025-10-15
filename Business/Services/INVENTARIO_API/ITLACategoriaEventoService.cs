

using Domain.INVENTARIO_API;

namespace Business.Services.INVENTARIO_API
{
    public class INVENTARIOCategoriaEventoService(INVENTARIOApiService api)
    {
        private readonly INVENTARIOApiService _api = api;

        public Task<GetCategoriaEventoByIdResponse?> GetByIdAsync(
            short Id,
            CancellationToken ct = default
        ) => _api.GetAsync<GetCategoriaEventoByIdResponse?>($"/internal/categoria-evento/{Id}", ct);
    }
}
