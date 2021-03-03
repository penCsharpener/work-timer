using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WorkTimer.MediatRTests
{
    [ExcludeFromCodeCoverage]
    internal class LoggerMock<T> : ILogger<T>
    {
        private readonly Stack<ReceivedLogEvent> _events = new Stack<ReceivedLogEvent>();

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        // source: https://github.com/nsubstitute/NSubstitute/issues/597#issuecomment-569273714
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _events.Push(new ReceivedLogEvent(logLevel, state.ToString()));
        }

        public void Received(LogLevel level, int count, string message = null)
        {
            var matchedEventsCount = _events.Count(e => e.Level == level && string.IsNullOrEmpty(message) || e.Message == message);

            if (matchedEventsCount != count)
            {
                throw new Exception($"Expected {count} call(s) to Log with the following arguments: {level}, \"{message}\". Actual received count: {matchedEventsCount}");
            }
        }
    }

    [ExcludeFromCodeCoverage]
    internal class ReceivedLogEvent
    {
        public ReceivedLogEvent(LogLevel level, string message)
        {
            Level = level;
            Message = message;
        }

        public LogLevel Level { get; }

        public string Message { get; }
    }
}