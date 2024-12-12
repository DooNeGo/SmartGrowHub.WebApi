using SmartGrowHub.Application.Services;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class TimeProvider : ITimeProvider
{
    public IO<DateTime> GetUtcNow() => lift(() => DateTime.UtcNow);
}
