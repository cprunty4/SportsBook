using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Entities
{
    public class Team
    {
        public string FullName { get; set; }

        public long Id { get; set; }
        public string LocationName { get; set; }
        public string NickName { get; set; }
        public string TeamAbbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public List<string> Colors { get; set; }
        public long? HomeStadiumId { get; set; }
        public string HelmetImageFileName { get; set; }
        public string LogoImage { get; set; }
        public int? EntityId { get; set; }
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public string LogoImageUri { get; set; }           
    }
}
