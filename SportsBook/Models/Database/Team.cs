﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Models.Database
{
    public class Team
    {
        public string TeamName => $"{Location} {Name}";

        public long Id { get; set; }
        public string Location { get; set; }
        public string Name { get; set; }
        public string TeamAbbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public List<string> Colors { get; set; }
        public long? HomeStadiumId { get; set; }

    }
}
