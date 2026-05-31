using System.Text.Json.Serialization;

namespace SmartGrowHub.Shared.GrowHubs.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DisabledScheduleDto), nameof(ScheduleTypeDto.Disabled))]
[JsonDerivedType(typeof(EnabledScheduleDto), nameof(ScheduleTypeDto.Enabled))]
[JsonDerivedType(typeof(DailyScheduleDto), nameof(ScheduleTypeDto.Daily))]
[JsonDerivedType(typeof(WeeklyScheduleDto), nameof(ScheduleTypeDto.Weekly))]
public abstract record ScheduleDto
{
    public T Match<T>(
        Func<DisabledScheduleDto, T> mapDisabled,
        Func<EnabledScheduleDto, T> mapEnabled,
        Func<DailyScheduleDto, T> mapDaily,
        Func<WeeklyScheduleDto, T> mapWeekly) =>
        this switch
        {
            DisabledScheduleDto program => mapDisabled(program),
            EnabledScheduleDto program => mapEnabled(program),
            DailyScheduleDto program => mapDaily(program),
            WeeklyScheduleDto program => mapWeekly(program),
            _ => throw new InvalidOperationException()
        };
}

public sealed record DisabledScheduleDto(string Id) : ScheduleDto;

public sealed record EnabledScheduleDto(string Id) : ScheduleDto;

public sealed record DailyScheduleDto(string Id, IReadOnlyList<ScheduleUnitDto<TimeOnly>> Entries) : ScheduleDto;

public sealed record WeeklyScheduleDto(string Id, IReadOnlyList<ScheduleUnitDto<WeekTimeOnlyDto>> Entries) : ScheduleDto;