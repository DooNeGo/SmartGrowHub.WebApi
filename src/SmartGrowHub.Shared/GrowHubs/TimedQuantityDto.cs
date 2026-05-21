namespace SmartGrowHub.Shared.GrowHubs;

public sealed record TimedQuantityDto<TTime>(Ulid Id, QuantityDto Quantity, TimeIntervalDto<TTime> Interval);