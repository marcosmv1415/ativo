using static APINotification.Controllers.AtivoController;

namespace APINotification.Models
{
    public class CurrentTradingPeriod
    {
        public TradingPeriod Pre { get; set; }
        public TradingPeriod Regular { get; set; }
        public TradingPeriod Post { get; set; }
    }
}
