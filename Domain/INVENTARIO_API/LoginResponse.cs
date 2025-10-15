
namespace Domain.INVENTARIO_API
{
    public class LoginResponse
    {
        public required DataObj Data {  get; set; }

        public class DataObj
        {
            public required String AccessToken { get; set; }
            public required String TokenType { get; set; }
        }
    }
}
