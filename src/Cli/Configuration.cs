using System.Collections.Generic;
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
}
