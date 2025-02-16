namespace SmartGrowHub.Application.Services;

public interface ITimeProvider
{
    IO<DateTime> UtcNow { get; }
}
