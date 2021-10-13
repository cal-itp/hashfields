using System;
using System.IO;

namespace HashFields.Cli.Services
{
    internal interface IConsoleService
    {
        Stream OpenStandardInput();
        Stream OpenStandardOutput();
    }

    internal class ConsoleService : IConsoleService
    {
        public Stream OpenStandardInput()
        {
            return Console.OpenStandardInput();
        }

        public Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }
    }

    internal class DevConsoleService : IConsoleService
    {
        public Stream OpenStandardInput()
        {
            return new MemoryStream();
        }

        public Stream OpenStandardOutput()
        {
            return Console.OpenStandardOutput();
        }
    }
}
