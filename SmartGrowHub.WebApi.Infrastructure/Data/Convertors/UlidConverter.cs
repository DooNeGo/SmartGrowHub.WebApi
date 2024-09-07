using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SmartGrowHub.WebApi.Infrastructure.Data.Convertors;

internal sealed class UlidConverter(ConverterMappingHints? mappingHints = null)
    : ValueConverter<Ulid, byte[]>(
            model => model.ToByteArray(),
            provider => new Ulid(provider),
            mappingHints: defaultHints.With(mappingHints))
{
    private static readonly ConverterMappingHints defaultHints = new(size: 16);

    public UlidConverter() : this(null) { }
}