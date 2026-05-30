using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(GrowHubModuleConfiguration))]
internal sealed class GrowHubModuleDb : IContainsId
{
    public required string Id { get; set; }
    
    public required ModuleTypeDb Type { get; set; }
    
    public required ModuleProgramDb Program { get; set; }
    
    public required string GrowHubId { get; set; }
    
    public GrowHubDb? GrowHub { get; set; }
}