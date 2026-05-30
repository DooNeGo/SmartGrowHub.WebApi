using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(ModuleProgramConfiguration))]
internal sealed class ModuleProgramDb : IContainsId
{
    public required string Id { get; set; }
    
    public required ProgramTypeDb Type { get; set; }
    
    public QuantityDb? ManualQuantity { get; set; }
    
    public List<TimedQuantityDb<TimeOnly>> TimeOnlyEntries { get; set; } = [];
    
    public List<TimedQuantityDb<WeekTimeOnlyDb>> WeekTimeOnlyEntries { get; set; } = [];
    
    public string? GrowHubModuleId { get; set; }
    
    public GrowHubModuleDb? GrowHubModule { get; set; }
}
