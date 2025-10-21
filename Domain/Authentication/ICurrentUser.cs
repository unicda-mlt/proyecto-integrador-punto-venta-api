

namespace Domain.Authentication
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }
        Guid? UsuarioId { get; }
        int? RolId { get; }
        string? Raw(string claimType);
    }
}
