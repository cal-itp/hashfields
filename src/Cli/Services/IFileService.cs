using System.IO;

namespace HashFields.Cli.Services
{
    internal interface IFileService
    {
        Stream OpenRead(string path);
        Stream OpenWrite(string path);
    }

    internal class FileService : IFileService
    {
        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public Stream OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }
    }
}
