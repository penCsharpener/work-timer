using System.Threading.Tasks;

namespace WorkTimer.Messaging.Abstractions
{
    public interface IMessageService
    {
        Task RecalculateStatsAsync(int userId);
    }
}
