using static APINotification.Controllers.AtivoController;

namespace APINotification.Models
{
    public class Result
    {
        public MetaData Meta { get; set; }
        public Indicators Indicators { get; set; }
        public long[] Timestamp { get; set; }
    }
}
