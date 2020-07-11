using System.Collections.Generic;

namespace SportsBook.Models
{
    public class TeamMetaData
    {
        public long TeamId { get; set; }
        public string TeamFullName { get; set; }
        public string TeamLocation { get; set; }
        public string TeamHelmetImageFileName { get; set; }
        public string TeamName { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public List<string> Comments { get; set; }
        public int EntityId { get; set; }
    }
}