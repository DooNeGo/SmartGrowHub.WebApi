using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Owned]
public sealed record WeekTimeOnlyDb(DayOfWeek DayOfWeek, TimeOnly TimeOnly);