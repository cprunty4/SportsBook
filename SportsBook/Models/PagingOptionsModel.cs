namespace SportsBook.Models
{
    public class PagingOptionsModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SortDirection SortDirection { get; set; }
        public string SortColumn { get; set; }
        public PagingOptionsModel()
        {
            Page = 1;
            PageSize = 200;
            SortDirection = SortDirection.ASC;
        }        
    }
}