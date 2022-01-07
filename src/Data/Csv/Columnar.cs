using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualBasic.FileIO;

namespace HashFields.Data.Csv
{
    /// <summary>
    /// Helper to work with delimited tabular data as columns rather than rows.
    /// </summary>
    /// <see cref="IEquatable{T}" />
    internal class Columnar : IEquatable<Columnar>
    {
        private readonly List<string> _header = new();
        private readonly Dictionary<string, List<string>> _data = new();

        /// <summary>
        /// The column of values by column name.
        /// </summary>
        /// <param name="key">The name of the column.</param>
        /// <returns>A list representing the column's values.</returns>
        public List<string> this[string key] { get => _data[key]; }

        /// <summary>
        /// The column of values by column index.
        /// </summary>
        /// <param name="index">The 0-based index of the column.</param>
        /// <returns>A list representing the column's values.</returns>
        public List<string> this[int index] { get => _data[_header[index]]; }

        /// <summary>
        /// The list of column names.
        /// </summary>
        public List<string> Header { get => _header.ToList(); }

        /// <summary>
        /// The list of data columns.
        /// </summary>
        public List<List<string>> Columns { get => _data.Values.ToList(); }

        /// <summary>
        /// Initialize a new <c>Columnar</c> for delimited data.
        /// </summary>
        /// <param name="stream">The <c>Stream</c> of data to read into this <c>Columnar</c>.</param>
        /// <param name="delimiter">The delimiter used between fields in the data.</param>
        public Columnar(Stream stream, string delimiter)
        {
            if (stream is not null)
            {
                var tuple = Parse(stream, delimiter);

                _header = tuple.Item1;
                _data = tuple.Item2;
            }
        }

        /// <summary>
        /// Call a function for each value in the specified columns.
        /// </summary>
        /// <param name="func">
        ///     A function taking a string as input and returning a string.
        ///     Each value in the column is passed through this function and
        ///     overwritten in-place.
        /// </param>
        /// <param name="columns">The list of columns to apply the function on.</param>
        public void Apply(Func<string, string> func, params string[] columns)
        {
            foreach (var column in _header.Intersect(columns).ToArray())
            {
                _data[column] = _data[column].ConvertAll(s => func(s));
            }
        }

        /// <summary>
        /// Remove the named columns from this <c>Columnar</c> data.
        /// The column names should match those found in the <c>Header</c>.
        /// </summary>
        /// <seealso cref="Header" />
        /// <param name="columns">The list of column names to remove.</param>
        public void Remove(params string[] columns)
        {
            // find intersection of the real header names and those for removal
            // create a new array from this intersection so we don't loop over
            // the collection we are modifying!
            foreach (var column in _header.Intersect(columns).ToArray())
            {
                _header.Remove(column);
                _data.Remove(column);
            }
        }

        /// <summary>
        /// Compute the list of data rows from the current state of this <c>Columnar</c>.
        /// </summary>
        public List<List<string>> Rows()
        {
            // find the column with the longest length (N) - the number of rows
            // create a list of N lists to represent the rows
            var rows = Enumerable.Range(0, Columns.Max(c => c.Count))
                                 .Select(_ => new List<string>())
                                 .ToList();

            foreach (var column in Columns)
            {
                // copy values for this column into each row
                for (int i = 0; i < column.Count; i++)
                {
                    // rows[i] is a list representing the ith row
                    // append the column value to the end of the row list
                    // the "next" position in the row
                    rows[i].Add(column[i]);
                }
            }

            // insert the header row first
            rows.Insert(0, Header);

            return rows;
        }

        /// <summary>
        /// Write this <c>Columnar</c> data to a stream as delimited tabular data.
        /// </summary>
        /// <param name="destination">A writable <c>Steam</c> target for this <c>Columnar</c>.</param>
        public void Write(Stream destination)
        {
            using var sw = new StreamWriter(destination);
            foreach (var row in Rows())
            {
                sw.WriteLine(String.Join(",", row));
            }
        }

        /// <summary>
        /// Read delimited data from a stream and convert into columnar format.
        /// </summary>
        /// <param name="stream">The source of data.</param>
        /// <param name="delimiter">The delimiter used to separate fields in the data.</param>
        /// <returns>A <c>Tuple</c> containing two items:
        ///     <list type="bullet">
        ///         <item>
        ///             <term><c>List{String}</c></term>
        ///             <description>The ordered header row of column names.</description>
        ///         </item>
        ///         <item>
        ///             <term><c>Dictionary{String,List{String}}</c></term>
        ///             <description>
        ///                 The data columns, where the key is the column name
        ///                 and the value is the list of values in the column.
        ///             </description>
        ///         </item>
        ///     </list>
        /// </returns>
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

        #region IEquatable<Columnar>

        public bool Equals(Columnar other)
        {
            if (other is null)
            {
                return false;
            }

            if (!_header.SequenceEqual(other._header))
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
            foreach (var header in _header)
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

        #endregion
    }
}
