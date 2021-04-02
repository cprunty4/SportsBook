namespace SportsBook.Entities
{
    public class Wager
    {
        public long Id { get; set; }
        public long GameId { get; set; }
        public int? WagerType { get; set; }
        public long? GameTeamId { get; set; }
        public decimal? WagerAmount { get; set; }        
        public decimal? WinAmount { get; set; }
        public decimal? PayoutAmount { get; set; }
        public string TeamName { get; set; }
        public string SpreadOfBet { get; set; }
        public string SpreadMoneylineOfBet { get; set; }
        public string MoneyLineOfBet { get; set; }
        public string OverUnderOfBet { get; set; }
        public string OverUnderMoneylineOfBet { get; set; }
        public string UpdatedBy { get; set; }
    }
}