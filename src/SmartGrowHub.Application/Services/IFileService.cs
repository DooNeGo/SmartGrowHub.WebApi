using System.Text;

namespace SmartGrowHub.Application.Services;

public interface IFileService
{
    IO<string> ReadAllText(string path, Encoding encoding);
}