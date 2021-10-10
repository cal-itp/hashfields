using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.VisualBasic.FileIO;

namespace HashFields.Data
{
    public class Csv
    {
        private readonly Stream _stream;

        public Csv(string csvText)
        {
            var text = (csvText ?? String.Empty).Trim();
            var data = Encoding.UTF8.GetBytes(text);

            _stream = new MemoryStream(data);
        }

        public Csv(Stream stream)
        {
            _stream = stream;
        }

        public IDictionary<string, List<string>> ToColumnar()
        {
            if (_stream is null || _stream.Length < 1)
            {
                return new Dictionary<string, List<string>>();
            }

            var columnar = new Dictionary<int, List<string>>();
            var header = new List<string>();

            using (var parser = new TextFieldParser(_stream))
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
            return columnar.ToDictionary(
                kvp => header[kvp.Key],
                kvp => kvp.Value
            );
        }
    }
}
