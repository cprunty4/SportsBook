using System;

namespace SportsBook.Entities
{
    public class Entity
    {
        public long ID { get; set; }
        public long EntityTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public long? SequenceNum { get; set; }
        public bool IsRequired { get; set; }
        public long? EntityFormControlTextMaskTypeId { get; set; }
        public long? EntityStatusId { get; set; }        
    }
}