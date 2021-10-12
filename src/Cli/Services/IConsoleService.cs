using System.IO;

namespace HashFields.Cli.Services
{
    internal interface IConsoleService
    {
        Stream OpenStandardInput();
        Stream OpenStandardOutput();
    }
}
