using SmartGrowHub.Application.Repositories;
using SmartGrowHub.AspNetCore.Modules.Extensions;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Shared.Results;
using static Microsoft.AspNetCore.Http.Results;
using static SmartGrowHub.AspNetCore.Modules.ErrorHandler;


namespace SmartGrowHub.AspNetCore.Modules.GrowHubs.Sensors.Endpoints;

public sealed class GetLatestSensorsMeasurementsEndpoint
{
    public static ValueTask<IResult> GetLatestSensorsMeasurements(string growHubId,
        ISensorMeasurementRepository repository, ILogger<GetLatestSensorsMeasurementsEndpoint> logger,
        CancellationToken cancellationToken) => (
            from id in Domain.Common.Id<GrowHub>.From(growHubId).ToIO()
            from measurements in repository.GetLatestByGrowHubId(id)
            select measurements.Select(x => x.ToDto()).AsEnumerable())
        .RunSafeAsync(EnvIO.New(token: cancellationToken))
        .Map(fin => fin.Match(
            dto => Ok(Result.Success(dto)),
            error => HandleError(logger, error)));
}