
namespace Core.Paging
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public string ContinuationToken { get; set; }
    }
}
