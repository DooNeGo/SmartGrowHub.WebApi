using System.Text;

namespace SmartGrowHub.Application.Services;

public interface IFileService
{
    IO<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken);
}