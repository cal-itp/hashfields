using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace HashFields.Cli.Options
{
    internal class StreamOptions
    {
        public const string ConfigurationSectionName = nameof(StreamOptions);

        public enum Types
        {
            File,
            StdIn,
            StdOut,
        }

        public class Channel
        {
            public string Path { get; set; }

            [EnumDataType(typeof(Types))]
            public string Type { get; set; }
        }

        public Channel Input { get; set; } = new Channel() { Type = nameof(Types.StdIn) };

        public Channel Output { get; set; } = new Channel() { Type = nameof(Types.StdOut) };
    }
}
