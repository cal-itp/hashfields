using System.IO;

namespace HashFields.Cli.Services
{
    internal interface IFileService
    {
        Stream OpenRead(string path);
        Stream OpenWrite(string path);
    }
}
