using EasyNetQ;
using System.Threading.Tasks;
using WorkTimer.Messaging.Abstractions;
using WorkTimer.Messaging.MessageModels;

namespace WorkTimer.Messaging.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IBus _bus;

        public MessageService(IBus bus)
        {
            _bus = bus;
        }

        public async Task RecalculateStatsAsync(int userId)
        {
            await _bus.PubSub.PublishAsync(new RecalculateStatsMessage(userId));
        }

        public async Task UpdateTotalHoursFromWorkDayAsync(int workDayId)
        {
            await _bus.PubSub.PublishAsync(new UpdateTotalHoursFromWorkDayMessage(workDayId));
        }

        public async Task UpdateOnEditWorkdayAsync(int workdayId, int userId)
        {
            await _bus.PubSub.PublishAsync(new UpdateOnEditWorkdayMessage(workdayId, userId));
        }
    }
}
