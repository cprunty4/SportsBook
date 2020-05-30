namespace SportsBook.Models
{
    public class Wager
    {
        public long Id { get; set; }
        public long GameId { get; set; }
        public WagerTypeEnum WagerType { get; set; }
        public decimal? OverUnder { get; set; }
        public int? OverUnderMoneyLine { get; set; }
        public decimal? Spread { get; set; }
        public int? SpreadMoneyLine { get; set; }
        public int? MoneyLine { get; set; }        

    }
}