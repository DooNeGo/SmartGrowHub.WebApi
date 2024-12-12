using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SmartGrowHub.Infrastructure.Data.Convertors;

internal sealed class UlidConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<Ulid, byte[]>(
            model => model.ToByteArray(),
            provider => new Ulid(provider),
            mappingHints: DefaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints DefaultHints = new(size: 16);

    public UlidConverter() : this(null) { }
}