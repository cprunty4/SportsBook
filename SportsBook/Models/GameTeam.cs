namespace SportsBook.Models
{
    public class GameTeam
    {
        public long TeamId { get; set; }
        public int Score { get; set; }
        public decimal Spread { get; set; }
        public int SpreadMoneyLine { get; set; }
        public int MoneyLine { get; set; }
    }
}