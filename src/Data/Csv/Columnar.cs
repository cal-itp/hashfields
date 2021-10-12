using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualBasic.FileIO;

namespace HashFields.Data.Csv
{
    internal class Columnar : IEquatable<Columnar>
    {
        private readonly List<string> _headers = new();
        private readonly Dictionary<string, List<string>> _data = new();

        public List<string> this[string key] { get => _data[key]; }
        public List<string> this[int index] { get => _data[_headers[index]]; }
        public List<string> Header { get => _headers.ToList(); }
        public List<List<string>> Columns { get => _data.Values.ToList(); }

        public Columnar(string delimiter) : this(new MemoryStream(), delimiter)
        {
        }

        public Columnar(Stream stream, string delimiter)
        {
            if (stream is not null)
            {
                var tuple = Parse(stream, delimiter);

                _headers = tuple.Item1;
                _data = tuple.Item2;
            }
        }

        public void Apply(Func<string, string> func, params string[] columns)
        {
            foreach (var column in _headers.Intersect(columns).ToArray())
            {
                _data[column] = _data[column].ConvertAll(s => func(s));
            }
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

        public void Remove(params string[] columns)
        {
            foreach (var column in _headers.Intersect(columns).ToArray())
            {
                _headers.Remove(column);
                _data.Remove(column);
            }
        }

        public void Write(Stream destination)
        {
            using var sw = new StreamWriter(destination);
            foreach (var row in Rows())
            {
                sw.WriteLine(String.Join(",", row));
            }
        }

        private List<List<string>> Rows()
        {
            var rows = Enumerable.Range(0, Columns.Max(c => c.Count))
                                 .Select(_ => new List<string>())
                                 .ToList();

            foreach (var column in Columns)
            {
                foreach (var val in column)
                {
                    rows[column.IndexOf(val)].Add(val);
                }
            }

            rows.Insert(0, Header);

            return rows;
        }

        private static Tuple<List<string>, Dictionary<string, List<string>>> Parse(Stream stream, string delimiter)
        {
            var header = new List<string>();
            var columnar = new Dictionary<int, List<string>>();
            using var parser = new TextFieldParser(stream);

            parser.SetDelimiters(new[] { delimiter });

            // first row assumed to be the header
            header = parser.ReadFields()?.ToList() ?? new List<string>();

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

            // convert index number back into header value
            return new Tuple<List<string>, Dictionary<string, List<string>>>(
                header,
                columnar.ToDictionary(
                    kvp => header[kvp.Key],
                    kvp => kvp.Value
                )
            );
        }
    }
}
