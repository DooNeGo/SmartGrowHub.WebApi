namespace SmartGrowHub.Shared.GrowHubs.Model;

public sealed record ScheduleUnitDto<T>(
    string Id,
    ScheduleUnitKindDto Kind,
    QuantityDto Quantity,
    TimeIntervalDto<T> Interval);