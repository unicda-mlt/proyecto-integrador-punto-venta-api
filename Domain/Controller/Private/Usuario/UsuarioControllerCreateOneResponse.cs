
using Domain.API;

namespace Domain.Controller.Private.Usuario
{
    public class UsuarioControllerCreateOneResponse : BaseObjectResponse<Data> {}

    public class Data
    {
        public Guid Id { get; set; }
    }
}
