using System;

namespace SportsBook.Models
{
    /// <summary>
    /// TODO Should probably be moved to Entities namespace
    /// </summary>
    public class EntityNote
    {
        public long ID { get; set; }
        public long EntityID { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }        
    }
}