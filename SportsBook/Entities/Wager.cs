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
        public decimal? SpreadOfBet { get; set; }
        public int? SpreadMoneylineOfBet { get; set; }
        public int? MoneyLineOfBet { get; set; }
        public double? OverUnderOfBet { get; set; }
        public int? OverUnderMoneylineOfBet { get; set; }
        public string UpdatedBy { get; set; }
    }
}