
namespace Domain.API
{
    public class BaseObjectResponse<T>
    {
        public bool Ok { get; set; } = false;
        public T? Data { get; set; } = default;
    }
}
