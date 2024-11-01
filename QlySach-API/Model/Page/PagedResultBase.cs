using QlySach_API.Data;

namespace QlySach_API.Model.Page
{
    public abstract class PagedResultBase
    {
        public int currentPage { get; set; }
        public int rowCount { get; set; }
        public int pageSize { get; set; }
        public int pageCount 
        {
            get 
            {
                return (int)Math.Ceiling((double)rowCount / pageSize);
            }
        }
        public int firstRowOnPage
        {
            get 
            {
                return (currentPage - 1) * pageSize + 1;
            }
        }

        public int lastRowOnPage
        {
            get
            {
                return Math.Min(currentPage *  pageSize, pageSize);
            }
        }
        public string? additionalData { get; set; } 

        public class pageResult
        {
            private readonly AppDbContext appDbContext;

            public pageResult(AppDbContext appDbContext)
            {
                this.appDbContext = appDbContext;
            }
        }
    }
}
