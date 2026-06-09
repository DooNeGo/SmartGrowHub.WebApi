namespace SmartGrowHub.Infrastructure.Data.Model;

internal interface IContainsId
{
    Ulid Id { get; set; }
}