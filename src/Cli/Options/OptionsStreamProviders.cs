using System;
using System.IO;

using HashFields.Cli.Services;
using HashFields.Data;
using Microsoft.Extensions.Options;

namespace HashFields.Cli.Options
{
    internal class OptionsInputStreamProvider : IStreamProvider
    {
        private readonly IConsoleService _console;

        private readonly IFileService _file;

        private readonly StreamOptions _options;

        public OptionsInputStreamProvider(
            IConsoleService console,
            IFileService file,
            IOptions<StreamOptions> options
            )
        {
            _console = console;
            _file = file;
            _options = options.Value;
        }

        public Stream Get() => _options.Input.Type switch
        {
            nameof(StreamOptions.Types.File) => _file.OpenRead(_options.Input.Path),
            nameof(StreamOptions.Types.StdIn) => _console.OpenStandardInput(),
            _ => throw new ArgumentOutOfRangeException(nameof(_options), "Input.Type"),
        };
    }

    internal class OptionsOutputStreamProvider : IStreamProvider
    {
        private readonly IConsoleService _console;

        private readonly IFileService _file;

        private readonly StreamOptions _options;

        public OptionsOutputStreamProvider(
            IConsoleService console,
            IFileService file,
            IOptions<StreamOptions> options
            )
        {
            _console = console;
            _file = file;
            _options = options.Value;
        }

        public Stream Get() => _options.Output.Type switch
        {
            nameof(StreamOptions.Types.File) => _file.OpenWrite(_options.Output.Path),
            nameof(StreamOptions.Types.StdOut) => _console.OpenStandardOutput(),
            _ => throw new ArgumentOutOfRangeException(nameof(_options), "Output.Type"),
        };
    }
}
