using SmartGrowHub.AspNetCore.Modules.Auth.Endpoints;

namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Modules.Endpoints;

public sealed class SetModuleModeEndpoint
{
    public static Task<IResult> SetModuleMode(string id, ILogger<SetModuleModeEndpoint> logger,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(Results.Ok());
        // return (
        //     from ulid in UlidFp.From(id));
    }
}