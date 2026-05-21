using System.Text;
using SmartGrowHub.Application.Services;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class FileService : IFileService
{
    public IO<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken) =>
        IO.liftAsync(() => File.ReadAllTextAsync(path, encoding, cancellationToken));
}