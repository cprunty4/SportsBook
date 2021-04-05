using System.Collections.Generic;

namespace SportsBook.Models
{
    public class GamesSearchResponse
    {
        public List<GameSlate> GameSlates { get; set; }
        public PagedResultModel pagedResult { get; set; }        
    }
}