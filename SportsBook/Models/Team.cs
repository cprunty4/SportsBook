using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Models
{
    public class Team
    {
        public long Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string TeamAbbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public List<string> Colors { get; set; }

    }
}
