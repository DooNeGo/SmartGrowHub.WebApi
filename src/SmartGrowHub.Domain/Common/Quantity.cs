using SmartGrowHub.Domain.Errors;

namespace SmartGrowHub.Domain.Common;

public readonly record struct Quantity(float Magnitude, MeasurementUnit Unit);

public static class QuantityDefaults
{
    public static Fin<Quantity> ValidatePowerPercentRange(Quantity quantity) =>
        quantity is { Magnitude: < 0 or > 100, Unit: MeasurementUnit.Percent }
            ? DomainErrors.CreateQuantityOutOfRangeError(quantity, 0, 100)
            : quantity;
}