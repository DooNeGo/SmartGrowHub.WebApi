using SmartGrowHub.Application.UseCases.GrowHubs;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;

namespace SmartGrowHub.AspNetCore.Modules.Extensions;

public static class ScheduleUnitTemplateExtensions
{
    public static ScheduleUnitTemplate<WeekTimeOnly> ToDomain(this ScheduleUnitTemplateDto<WeekTimeOnlyDto> template) =>
        new(template.Kind.ToDomain(), template.Quantity.ToDomain(), template.Interval.ToDomain());
    
    public static ScheduleUnitTemplate<TimeOnlyWrapper> ToDomain(this ScheduleUnitTemplateDto<TimeOnly> template) =>
        new(template.Kind.ToDomain(), template.Quantity.ToDomain(), template.Interval.ToDomain());
}