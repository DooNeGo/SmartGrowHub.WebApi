using System.Text;
using SmartGrowHub.Application.Services;

namespace SmartGrowHub.Infrastructure.Services;

internal sealed class FileService : IFileService
{
    public IO<string> ReadAllTextAsync(string path, Encoding encoding) =>
        IO.liftAsync(env => File.ReadAllTextAsync(path, encoding, env.Token));
}