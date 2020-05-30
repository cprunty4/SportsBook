using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsBook.Models
{
    public class Game
    {
        public long Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public GameTypeEnum GameType { get; set; }
        public int LeagueId { get; set; }
        public long AwayTeamId { get; set; }
        public long HomeTeamId { get; set; }
        public long StadiumId { get; set; }
        public int Season { get; set; }


    }
}
