namespace SportsBook.Models
{
    public class PagedResultModel
    {
        public PagingOptionsModel PagingOptions { get; set; }
        public long TotalResultCount { get; set; }
        public bool HasMoreResults { get; set; }
        public PagedResultModel()
        {
            PagingOptions = new PagingOptionsModel();
        }        
    }
}