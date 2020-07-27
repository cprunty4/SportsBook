using System.Collections.Generic;
using SportsBook.Entities;

namespace SportsBook.Models
{
    public class CommentsData
    {
        public List<EntityNote> Comments {get; set;}
        public string TeamName { get; set; } 
        public string LogoImage { get; set; }     
        public int NumberOfComments { get; set; }
        public string TeamLocation { get; set; }                  

    }
}