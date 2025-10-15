
namespace Domain.API.Interfaces
{
    public interface IGetAllReturn<T>
    {
        public IPagination Pagination { get; set; }
        public List<T> Data { get; set; }
    }
}
