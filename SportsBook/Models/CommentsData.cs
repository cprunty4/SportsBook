using System.Collections.Generic;

namespace SportsBook.Models
{
    public class CommentsData
    {
        public List<EntityNote> Comments {get; set;}
        public string TeamName { get; set; } 
        public string TeamHelmetImageFileName { get; set; }     
        public int NumberOfComments { get; set; }
        public string TeamLocation { get; set; }                  

    }
}