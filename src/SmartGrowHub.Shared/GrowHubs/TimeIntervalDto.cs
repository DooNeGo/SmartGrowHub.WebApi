namespace SmartGrowHub.Shared.GrowHubs;

public readonly record struct TimeIntervalDto<TTime>(TTime Start, TTime End);