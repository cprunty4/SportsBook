using System;

namespace SportsBook.Entities
{
    /// <summary>
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