﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Models
{
    public class Game
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int LeagueId { get; set; }
        public GameTypeEnum GameType { get; set; }
        public long AwayGameTeamId { get; set; }
        public long HomeGameTeamId { get; set; }
        public long StadiumId { get; set; }
        public int SeasonYear { get; set; }
        public int WeekNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public bool IsFinal { get; set; }
        public bool IsStarted { get; set; }
        public string Weather { get; set; }
        public string Wind { get; set; }
        public decimal OverUnder { get; set; }
        public int OverUnderMoneyLine { get; set; }

    }
}
