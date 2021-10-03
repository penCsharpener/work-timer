namespace WorkTimer.Messaging.MessageModels
{
    public class UpdateTotalHoursFromWorkDayMessage
    {
        public int WorkdayId { get; set; }

        public UpdateTotalHoursFromWorkDayMessage(int workdayId)
        {
            WorkdayId = workdayId;
        }
    }
}
