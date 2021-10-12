using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

using HashFields.Data;

namespace HashFields.Cli
{
    internal class DataOptions
    {
        public const string ConfigurationSectionName = nameof(DataOptions);

        public string Delimiter { get; set; } = ",";

        public string HashAlgorithm { get; set; } = StringHasher.SupportedAlgorithms.Last();

        public IEnumerable<string> Drop { get; set; }

        public IEnumerable<string> Skip { get; set; }
    }

    internal class StreamOptions
    {
        public const string ConfigurationSectionName = nameof(StreamOptions);

        public enum Types
        {
            File,
            StdIn,
            StdOut,
        }

        public class Options
        {
            public string Channel { get; set; }

            [EnumDataType(typeof(Types))]
            public string Type { get; set; }
        }

        public Options Input { get; set; }

        public Stream InputStream() => Input.Type switch
        {
            nameof(Types.StdIn) => Console.OpenStandardInput(),
            _ => throw new ArgumentOutOfRangeException(),
        };

        public Stream OutputStream() => Output.Type switch
        {
            nameof(Types.StdOut) => Console.OpenStandardOutput(),
            _ => throw new ArgumentOutOfRangeException(),
        };

        public Options Output { get; set; }
    }
}
