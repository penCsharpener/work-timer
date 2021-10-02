namespace WorkTimer.Messaging.MessageModels
{
    public class RecalculateStatsMessage
    {
        public int UserId { get; set; }

        public RecalculateStatsMessage(int userId)
        {
            UserId = userId;
        }
    }
}
