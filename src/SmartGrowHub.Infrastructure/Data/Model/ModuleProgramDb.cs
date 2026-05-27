namespace SmartGrowHub.Infrastructure.Data.Model;

internal sealed class ModuleProgramDb
{
    public required Ulid Id { get; set; }
    
    public required ProgramTypeDb Type { get; set; }
    
    public required Ulid GrowHubModuleId { get; set; }
    
    public GrowHubModuleDb? GrowHubModule { get; set; }
    
    public List<TimedQuantityDb> Entries { get; set; } = [];
}
