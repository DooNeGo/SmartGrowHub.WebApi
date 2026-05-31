using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Owned]
internal sealed class IntervalDb
{
    public required string Start { get; set; }
    
    public required string End { get; set; }
}