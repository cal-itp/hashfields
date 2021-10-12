using System.IO;
using HashFields.Cli.Services;

namespace HashFields.Cli.Tests.Fakes
{
    public class FakeConsoleService : IConsoleService
    {
        public Stream OpenStandardInput()
        {
            return new MemoryStream();
        }

        public Stream OpenStandardOutput()
        {
            return new MemoryStream();
        }
    }
}
