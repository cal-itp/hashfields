using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualBasic.FileIO;

namespace HashFields.Data
{
    public interface IColumnar
    {
        Columnar Columnar { get; }
    }

    public class Columnar : IEquatable<Columnar>
    {
        private readonly List<string> _headers = new();
        private readonly Dictionary<string, List<string>> _data = new();

        public List<string> this[string key] { get => _data[key]; }
        public List<string> this[int index] { get => _data[_headers[index]]; }
        public IList<string> Header { get => _headers.ToList(); }
        public IList<List<string>> Columns { get => _data.Values.ToList(); }

        public Columnar() : this(null)
        {
        }

        public Columnar(Stream stream)
        {
            if (stream?.Length > 0)
            {
                var tuple = Parse(stream);

                _headers = tuple.Item1;
                _data = tuple.Item2;
            }
        }

        private static Tuple<List<string>, Dictionary<string, List<string>>> Parse(Stream stream)
        {
            var header = new List<string>();
            var columnar = new Dictionary<int, List<string>>();

            using (var parser = new TextFieldParser(stream))
            {
                parser.SetDelimiters(new[] { "," });

                // first row assumed to be the header
                header = parser.ReadFields().ToList();

                foreach (var field in header)
                {
                    // internally track the index to ensure order is maintained
                    columnar.Add(header.IndexOf(field), new List<string>());
                }

                while (!parser.EndOfData)
                {
                    // read the next line of data
                    // add each field's value to the corresponding column list
                    var fields = parser.ReadFields();

                    foreach (var key in columnar.Keys)
                    {
                        columnar[key].Add(fields[key]);
                    }
                }
            }

            // convert index number back into header value
            return new Tuple<List<string>, Dictionary<string, List<string>>>(
                header,
                columnar.ToDictionary(
                    kvp => header[kvp.Key],
                    kvp => kvp.Value
                )
            );
        }

        public bool Equals(Columnar other)
        {
            if (other is null)
            {
                return false;
            }

            if (!_headers.SequenceEqual(other._headers))
            {
                return false;
            }

            foreach (var column in _data)
            {
                if (!column.Value.SequenceEqual(other._data[column.Key]))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is not Columnar columnar)
            {
                return false;
            }

            return Equals(columnar);
        }

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            foreach (var header in _headers)
            {
                hashcode.Add(header);
            }
            foreach (var column in _data.Values)
            {
                foreach (var val in column)
                {
                    hashcode.Add(val);
                }
            }
            return hashcode.ToHashCode();
        }
    }
}
