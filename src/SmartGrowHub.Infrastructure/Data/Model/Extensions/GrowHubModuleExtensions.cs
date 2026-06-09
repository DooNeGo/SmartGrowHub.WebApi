using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class GrowHubModuleExtensions
{
    public static GrowHubModuleDb ToDb(this GrowHubModule module) => new()
    {
        Id = module.Id,
        Type = module.Type.ToDb(),
        Schedule = module.Schedule.ToDb(),
        GrowHubId = module.GrowHubId
    };
    
    public static Fin<GrowHubModule> ToDomain(this GrowHubModuleDb module) =>
        from program in module.Schedule.ToDomain()
        let type = module.Type.ToDomain()
        let id = new Id<GrowHubModule>(module.Id)
        let growHubId = new Id<GrowHub>(module.GrowHubId)
        select new GrowHubModule(id, growHubId, program, type);
}