using SmartGrowHub.Domain.Common;

namespace SmartGrowHub.Infrastructure.Data.Model.Extensions;

internal static class MeasurementUnitExtensions
{
    public static MeasurementUnitDb ToDb(this MeasurementUnit unit) => unit switch
    {
        MeasurementUnit.Celsius => MeasurementUnitDb.Celsius,
        MeasurementUnit.Percent => MeasurementUnitDb.Percent,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
    
    public static MeasurementUnit ToDomain(this MeasurementUnitDb unit) => unit switch
    {
        MeasurementUnitDb.Celsius => MeasurementUnit.Celsius,
        MeasurementUnitDb.Percent => MeasurementUnit.Percent,
        _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
    };
}