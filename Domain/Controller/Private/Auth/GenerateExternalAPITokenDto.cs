
namespace Domain.Controller.Private.Auth
{
    public class GenerateExternalAPITokenDto
    {
        public required short InstitucionExternaId { get; set; }
        public required string Token { get; set; }
    }
}
