using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Owned]
internal sealed class IntervalDb<T>
{
    public required T Start { get; set; }
    
    public required T End { get; set; }
}