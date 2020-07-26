using System;

namespace SportsBook.Models
{
    /// <summary>
    /// TODO Should probably be moved to Entities namespace
    /// </summary>
    public class EntityStatusHistory
    {
        public int id { get; set; } 
        public int entityID { get; set; } 
        public string status { get; set; } 
        public DateTime insertedDate { get; set; } 
        public DateTime updatedDate { get; set; } 
        public string updatedBy { get; set; }        
    }
}