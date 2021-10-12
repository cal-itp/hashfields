using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HashFields.Data.Csv
{
    public class Csv : IColumnOperator, IStreamWriter
    {
        internal static readonly string DefaultDelimiter = ",";

        internal readonly Columnar _columnar;

        internal string Delimiter { get; set; } = DefaultDelimiter;

        public List<string> Header => _columnar.Header.ToList();

        public Csv(IStreamProvider streamProvider, string delimiter = null)
        {
            Delimiter = delimiter ?? Delimiter;

            using var stream = streamProvider?.Get() ?? new MemoryStream();
            _columnar = new Columnar(stream, Delimiter);
        }

        public Csv(string csvText, string delimiter = null)
        {
            var text = (csvText ?? String.Empty).Trim();
            var data = Encoding.UTF8.GetBytes(text);

            Delimiter = delimiter ?? Delimiter;

            using var stream = new MemoryStream(data);
            _columnar = new Columnar(stream, Delimiter);
        }

        public void Apply(Func<string, string> func, params string[] columns)
        {
            _columnar.Apply(func, columns);
        }

        public void Remove(params string[] columns)
        {
            _columnar.Remove(columns ?? Array.Empty<string>());
        }

        public void Write(Stream destination)
        {
            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _columnar.Write(destination);
        }
    }
}
