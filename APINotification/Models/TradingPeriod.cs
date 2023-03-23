namespace APINotification.Models
{
    public class TradingPeriod
    {
        public string Timezone { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public int Gmtoffset { get; set; }
    }
}
