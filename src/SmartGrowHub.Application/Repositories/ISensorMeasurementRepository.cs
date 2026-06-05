using System.Collections.Immutable;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Application.Repositories;

public interface ISensorMeasurementRepository : IRepository<SensorMeasurement>
{
    IO<ImmutableList<SensorMeasurement>> GetLatestByGrowHubId(Id<GrowHub> growHubId);
}