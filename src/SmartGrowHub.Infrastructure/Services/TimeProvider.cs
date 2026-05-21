using SmartGrowHub.Application.Services;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class TimeProvider : ITimeProvider
{
    public IO<DateTime> UtcNow { get; } = IO.lift(() => DateTime.UtcNow);
}
