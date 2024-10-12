using SmartGrowHub.Application.Services;

namespace SmartGrowHub.WebApi.Infrastructure.Services;

internal sealed class TimeProvider : ITimeProvider
{
    public IO<DateTime> GetUtcNow() => lift(() => DateTime.UtcNow);
}
