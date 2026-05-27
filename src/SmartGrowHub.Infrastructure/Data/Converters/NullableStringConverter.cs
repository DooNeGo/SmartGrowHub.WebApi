using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SmartGrowHub.Infrastructure.Data.Converters;

internal sealed class NullableStringConverter()
    : ValueConverter<string, string?>(
        model => string.IsNullOrWhiteSpace(model) ? null : model,
        provider => provider ?? string.Empty)
{
    public override bool ConvertsNulls => true;
}