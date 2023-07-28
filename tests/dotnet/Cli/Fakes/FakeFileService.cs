using System.IO;
using HashFields.Cli.Services;

namespace HashFields.Cli.Tests.Fakes
{
    public class FakeFileService : IFileService
    {
        public Stream OpenRead(string path)
        {
            return new MemoryStream();
        }

        public Stream OpenWrite(string path)
        {
            return new MemoryStream();
        }
    }
}
