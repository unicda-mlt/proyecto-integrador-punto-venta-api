using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Authentication;

namespace Business
{

    public sealed class CurrentUser(IHttpContextAccessor ctx) : ICurrentUser
    {
        private readonly IHttpContextAccessor _ctx = ctx;
        private ClaimsPrincipal? Principal => _ctx.HttpContext?.User;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;

        public Guid? UsuarioId
        {
            get
            {
                var id = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(id, out var g) ? g : null;
            }
        }

        public int? RolId
        {
            get
            {
                var r = Principal?.FindFirstValue(ClaimTypes.Role);
                return int.TryParse(r, out var i) ? i : null;
            }
        }

        public string? Raw(string claimType) => Principal?.FindFirst(claimType)?.Value;
    }
}
