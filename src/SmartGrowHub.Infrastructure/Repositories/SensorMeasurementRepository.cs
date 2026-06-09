using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using SmartGrowHub.Application.Repositories;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Infrastructure.Data;
using SmartGrowHub.Infrastructure.Data.Model;
using SmartGrowHub.Infrastructure.Data.Model.Extensions;

namespace SmartGrowHub.Infrastructure.Repositories;

internal sealed class SensorMeasurementRepository :
    Repository<SensorMeasurement, SensorReadingDb>,
    ISensorMeasurementRepository
{
    private readonly ApplicationContext _context;
    
    public SensorMeasurementRepository(ApplicationContext context) : base(context)
    {
        _context = context;
    }

    public IO<ImmutableList<SensorMeasurement>> GetLatestByGrowHubId(Id<GrowHub> growHubId) =>
        from dbs in IO.liftAsync(env => AddIncludes(_context.Set<SensorReadingDb>()
                .Where(x => x.GrowHubId == growHubId.Value))
            .GroupBy(x => x.SensorId)
            .Select(grouping => grouping
                .OrderByDescending(x => x.CreatedAt)
                .First())
            .ToListAsync(env.Token))
        from domains in dbs
            .AsIterable()
            .Traverse(ToDomain)
            .As().ToIO()
        select domains.ToImmutableList();

    protected override SensorReadingDb ToDb(SensorMeasurement domain)
    {
        return new SensorReadingDb
        {
            Id = domain.Id,
            SensorId = domain.SensorId,
            Type = ToDb(domain.Type),
            Quantity = domain.Quantity.ToDb(),
            CreatedAt = domain.CreatedAt,
            GrowHubId = domain.GrowHubId
        };
    }
    
    private static SensorTypeDb ToDb(SensorType type) => type switch
    {
        SensorType.RandomNumber => SensorTypeDb.RandomNumber,
        SensorType.AirTemperature => SensorTypeDb.AirTemperature,
        SensorType.AirPressure => SensorTypeDb.AirPressure,
        SensorType.AirHumidity => SensorTypeDb.AirHumidity,
        SensorType.PlantHeight => SensorTypeDb.PlantHeight,
        SensorType.SoilMoisture => SensorTypeDb.SoilMoisture,
        SensorType.SoilTemperature => SensorTypeDb.SoilTemperature,
        SensorType.Illumination => SensorTypeDb.Illumination,
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected sensor type value: {type}")
    };

    protected override Fin<SensorMeasurement> ToDomain(SensorReadingDb db) =>
        from sensorId in NonEmptyString.From(db.SensorId)
        let id = new Id<SensorMeasurement>(db.Id)
        let growHubId = new Id<GrowHub>(db.GrowHubId)
        select new SensorMeasurement(
            id, growHubId, sensorId, ToDomain(db.Type), db.Quantity.ToDomain(), db.CreatedAt);

    private static SensorType ToDomain(SensorTypeDb type) => type switch
    {
        SensorTypeDb.RandomNumber => SensorType.RandomNumber,
        SensorTypeDb.AirTemperature => SensorType.AirTemperature,
        SensorTypeDb.AirPressure => SensorType.AirPressure,
        SensorTypeDb.AirHumidity => SensorType.AirHumidity,
        SensorTypeDb.PlantHeight => SensorType.PlantHeight,
        SensorTypeDb.SoilMoisture => SensorType.SoilMoisture,
        SensorTypeDb.SoilTemperature => SensorType.SoilTemperature,
        SensorTypeDb.Illumination => SensorType.Illumination,
        _ => throw new ArgumentOutOfRangeException(nameof(type), $"Not expected sensor type value: {type}")
    };

    protected override IQueryable<SensorReadingDb> AddIncludes(IQueryable<SensorReadingDb> query) => query;
}