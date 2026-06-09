using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(GrowHubModuleConfiguration))]
internal sealed class GrowHubModuleDb : IContainsId
{
    public required Ulid Id { get; set; }
    
    public required ModuleTypeDb Type { get; set; }
    
    public required ScheduleDb Schedule { get; set; }
    
    public required Ulid GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}