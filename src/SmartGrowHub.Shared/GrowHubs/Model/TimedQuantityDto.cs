namespace SmartGrowHub.Shared.GrowHubs.Model;

public sealed record TimedQuantityDto<T>(QuantityDto Quantity, TimeIntervalDto<T> Interval);