namespace SportsBook.Models
{
    public class Wager
    {
        public long Id { get; set; }
        public long GameId { get; set; }
        public WagerTypeEnum WagerType { get; set; }
        public long? GameTeamId { get; set; }
        public decimal WagerAmount { get; set; }        
        public decimal WagerAmountToWin { get; set; }
        public decimal? SpreadOfBet { get; set; }
        public int? SpreadMoneylineOfBet { get; set; }
        public int? MoneyLineOfBet { get; set; }
        public decimal? OverUnderOfBet { get; set; }
        public int? OverUnderMoneylineOfBet { get; set; }
    }
}