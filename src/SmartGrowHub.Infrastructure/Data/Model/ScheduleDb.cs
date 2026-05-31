using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(ScheduleConfiguration))]
internal sealed class ScheduleDb : IContainsId
{
    public required string Id { get; set; }
    
    public required ScheduleTypeDb Type { get; set; }
    
    public ICollection<ScheduleUnitDb> Units { get; set; } = [];
    
    public required string GrowHubModuleId { get; set; }
    
    public GrowHubModuleDb? GrowHubModule { get; set; }
}
