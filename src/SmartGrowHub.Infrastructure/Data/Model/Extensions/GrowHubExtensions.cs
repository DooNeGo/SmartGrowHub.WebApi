using System.Collections.Immutable;
using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class GrowHubExtensions
{
    public static GrowHubDb ToDb(this GrowHub growHub) => new()
    {
        Id = growHub.Id,
        UserId = growHub.UserId,
        Name = growHub.Name,
        Model = growHub.Model,
        Plant = growHub.Plant.ValueUnsafe()?.ToDb(),
        Modules = growHub.Modules.Select(x => x.ToDb()).ToList()
    };
    
    public static Fin<GrowHub> ToDomain(this GrowHubDb growHub) =>
        from name in NonEmptyString.From(growHub.Name)
        from model in NonEmptyString.From(growHub.Model)
        let id = new Id<GrowHub>(growHub.Id)
        let userId = new Id<User>(growHub.UserId)
        from modules in growHub.Modules.AsIterable().Traverse(x => x.ToDomain())
        from plant in Prelude.Optional(growHub.Plant).Traverse(x => x.ToDomain())
        select new GrowHub(id, userId, name, model, modules.ToImmutableList(), plant);
}