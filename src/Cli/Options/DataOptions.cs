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

        public bool HyphenateHashes { get; set; } = true;

        public bool LowercaseHashes { get; set; } = false;

        public List<string> Drop { get; } = new List<string>();

        public List<string> Skip { get; } = new List<string>();
    }
}
