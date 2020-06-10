using System;

namespace SportsBook.Models.Views
{
    public class GameSlate
    {
        public string LeagueName { get; set; }
        public string GameType { get; set; }
        public int WeekNumber { get; set; }
        public int SeasonYear { get; set; }
        public string AwayTeamName { get; set; }
        
        public decimal AwayTeamSpread { get; set; }
        public int AwayTeamSpreadMoneyline { get; set; }
        public int AwayTeamMoneyline { get; set; }
        public string HomeTeamName { get; set; }

        public decimal HomeTeamSpread { get; set; }
        public int HomeTeamSpreadMoneyline { get; set; }
        public int HomeTeamMoneyline { get; set; }
        public decimal OverUnder { get; set; }
        public int OverUnderMoneyline { get; set; }
        public DateTime? GameStartDateTime { get; set; }
        public string StadiumName { get; set; }
        public string StadiumImageFileName { get; set; }

    }
}