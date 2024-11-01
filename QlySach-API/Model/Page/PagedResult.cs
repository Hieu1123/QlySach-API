namespace QlySach_API.Model.Page
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T>? Results { get; set; } 

        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}
