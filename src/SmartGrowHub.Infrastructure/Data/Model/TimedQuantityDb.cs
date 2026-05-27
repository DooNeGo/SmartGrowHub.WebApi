using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Infrastructure.Data.Configurations;

namespace SmartGrowHub.Infrastructure.Data.Model;

[EntityTypeConfiguration(typeof(TimedQuantityConfiguration))]
internal sealed class TimedQuantityDb
{
    public required Ulid Id { get; set; }
    
    public required float Magnitude { get; set; }
    
    public required UnitDb Unit { get; set; }
    
    public required string StartTime { get; set; } = string.Empty;
    
    public required string EndTime { get; set; } = string.Empty;
    
    public required Ulid ModuleProgramId { get; set; }
    
    public ModuleProgramDb? ModuleProgram { get; set; }
}