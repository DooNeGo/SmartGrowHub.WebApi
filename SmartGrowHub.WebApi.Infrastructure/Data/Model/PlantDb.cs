namespace SmartGrowHub.WebApi.Infrastructure.Data.Model;

internal sealed record PlantDb(
    Ulid Id, string Name)
{
    private PlantDb() : this(
        default!, default!)
    { }  // Used by EF Core
}