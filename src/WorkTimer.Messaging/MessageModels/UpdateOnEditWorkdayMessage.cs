namespace WorkTimer.Messaging.MessageModels;

public class UpdateOnEditWorkdayMessage
{
    public int WorkdayId { get; set; }
    public int UserId { get; set; }

    public UpdateOnEditWorkdayMessage(int workdayId, int userId)
    {
        WorkdayId = workdayId;
        UserId = userId;
    }
}
