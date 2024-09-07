namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record GrowHubDb(
    Ulid Id,
    IEnumerable<SensorReadingDb> SensorReadings,
    IEnumerable<SettingDb> Settings,
    Ulid? PlantId,
    PlantDb? Plant,
    Ulid UserId,
    UserDb User)
{
    private GrowHubDb() : this(
        default!, [], [],
        default, default!,
        default, default!)
    { }   // Used by EF Core
}
