namespace SportsBook.Entities
{
    public class GameTeam
    {
        public long Id { get; set; }
        public long TeamId { get; set; }
        public int? Score { get; set; }
        public decimal SpreadCurrent { get; set; }
        public int SpreadMoneylineCurrent { get; set; }
        public int MoneyLineCurrent { get; set; }
    }
}