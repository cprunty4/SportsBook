namespace SportsBook.Models
{
    public class CreateWager
    {
        public string AwayTeamFullName { get; set; }
        public string HomeTeamFullName { get; set; }
        public string WagerGameTeamTeamName { get; set; }
        public decimal? WagerGameTeamSpreadOfBet { get; set; }
        public int? WagerGameTeamSpreadMoneylineOfBet { get; set; }
        public decimal? WagerAmount { get; set; }
        public decimal? WagerAmountToWin { get; set; }

    }
}