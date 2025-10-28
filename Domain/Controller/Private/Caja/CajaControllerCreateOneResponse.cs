
using Domain.API;

namespace Domain.Controller.Private.Caja
{
    public class CajaControllerCreateOneResponse : BaseObjectResponse<Data> {}

    public class Data
    {
        public Guid Id { get; set; }
    }
}
