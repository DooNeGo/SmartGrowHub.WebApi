using Microsoft.EntityFrameworkCore;

namespace SmartGrowHub.Infrastructure.Data.Model;

[Owned]
internal sealed record QuantityDb(float Magnitude, MeasurementUnitDb Unit);