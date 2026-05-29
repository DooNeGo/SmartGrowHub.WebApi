using System.Text.Json.Serialization;

namespace SmartGrowHub.Shared.GrowHubs.Model;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DisabledProgramDto), nameof(ProgramTypeDto.Disabled))]
[JsonDerivedType(typeof(ManualProgramDto), nameof(ProgramTypeDto.Manual))]
[JsonDerivedType(typeof(DailyProgramDto), nameof(ProgramTypeDto.Daily))]
[JsonDerivedType(typeof(WeeklyProgramDto), nameof(ProgramTypeDto.Weekly))]
public abstract record ModuleProgramDto
{
    public T Match<T>(
        Func<DisabledProgramDto, T> mapDisabled,
        Func<ManualProgramDto, T> mapManual,
        Func<DailyProgramDto, T> mapDaily,
        Func<WeeklyProgramDto, T> mapWeekly) =>
        this switch
        {
            DisabledProgramDto program => mapDisabled(program),
            ManualProgramDto program => mapManual(program),
            DailyProgramDto program => mapDaily(program),
            WeeklyProgramDto program => mapWeekly(program),
            _ => throw new InvalidOperationException()
        };
}

public sealed record DisabledProgramDto : ModuleProgramDto;

public sealed record ManualProgramDto(string Id, QuantityDto Quantity) : ModuleProgramDto;

public sealed record DailyProgramDto(string Id, IReadOnlyList<TimedQuantityDto<TimeOnly>> Entries) : ModuleProgramDto;

public sealed record WeeklyProgramDto(string Id, IReadOnlyList<TimedQuantityDto<WeekTimeOnlyDto>> Entries) : ModuleProgramDto;