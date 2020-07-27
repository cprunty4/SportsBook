using System;
using System.Collections.Generic;

namespace SportsBook.Models
{
    public class TeamMetaData
    {
        private DateTime lastCommentUpdatedDate;

        public long TeamId { get; set; }
        public string TeamFullName { get; set; }
        public string TeamLocation { get; set; }
        public string LogoImage { get; set; }
        public string TeamName { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public List<string> Comments { get; set; }
        public int EntityId { get; set; }
        public DateTime LastCommentUpdatedDate { get => lastCommentUpdatedDate; set => lastCommentUpdatedDate = value; }
    }
}