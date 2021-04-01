namespace SportsBook.Models
{
    public class CreateWager
    {
        public string AwayTeamFullName { get; set; }
        public string HomeTeamFullName { get; set; }
        public string WagerGameTeamTeamName { get; set; }
        public string WagerGameTeamSpreadOfBet { get; set; }
        public string WagerGameTeamSpreadMoneylineOfBet { get; set; }
        public decimal? WagerAmount { get; set; }
        public decimal? WagerAmountToWin { get; set; }
        public int? WagerType { get; set; }

    }
}