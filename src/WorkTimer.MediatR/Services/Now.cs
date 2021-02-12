using System;
using WorkTimer.MediatR.Services.Abstractions;

namespace WorkTimer.MediatR.Services
{
    public class NowTimeProvider : INow
    {
        public DateTime Now => DateTime.Now;
    }
}
