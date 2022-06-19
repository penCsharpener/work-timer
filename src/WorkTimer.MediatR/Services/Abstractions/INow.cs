using System;

namespace WorkTimer.MediatR.Services.Abstractions;

public interface INow
{
    DateTime Now { get; }
}
