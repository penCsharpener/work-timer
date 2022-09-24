using System.Threading.Tasks;

namespace WorkTimer.Messaging.Abstractions;

public interface IMessageService
{
    Task RecalculateStatsAsync(int userId);
    Task UpdateOnEditWorkdayAsync(int workdayId, int userId);
    Task UpdateTotalHoursFromWorkDayAsync(int workDayId);
}
