using System;

namespace SportsBook.Entities
{
    /// <summary>
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