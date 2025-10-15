using Domain.API.Interfaces;

namespace Domain.API
{
    public class GetAllReturn<T> : IGetAllReturn<T>
    {
        public required IPagination Pagination { get; set; }
        public required List<T> Data { get; set; }
    }
}
