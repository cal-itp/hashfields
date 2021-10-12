using System.Collections.Generic;
using System.Linq;

using HashFields.Data;

namespace HashFields.Cli.Options
{
    internal class DataOptions
    {
        public const string ConfigurationSectionName = nameof(DataOptions);

        public string Delimiter { get; set; } = ",";

        public string HashAlgorithm { get; set; } = StringHasher.SupportedAlgorithms.Last();

        public List<string> Drop { get; } = new List<string>();

        public List<string> Skip { get; } = new List<string>();
    }
}
