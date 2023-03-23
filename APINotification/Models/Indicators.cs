using static APINotification.Controllers.AtivoController;

namespace APINotification.Models
{
    public class Indicators
    {
        public Quote[] Quote { get; set; }
        public AdjClose[] Adjclose { get; set; }
    }
}
