using System;

namespace SportsBook.Models
{
    public class GamesSearchRequest
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string teamName { get; set; }
        public int seasonYear { get; set; }
        public PagingOptionsModel pagingOptions { get; set; }        
    }
}