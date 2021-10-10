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
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly byte[] _csv_data;
        private readonly string _csv_text;

        public Csv(string csv_text)
        {
            _csv_text = (csv_text ?? String.Empty).Trim();
            _csv_data = _encoding.GetBytes(_csv_text);
        }

        public IDictionary<string, List<string>> ToColumnar()
        {
            if (String.IsNullOrEmpty(_csv_text))
            {
                return new Dictionary<string, List<string>>();
            }

            var columnar = new Dictionary<int, List<string>>();
            var header = new List<string>();

            // convert raw _csv_data bytes to MemoryStream for TextFieldParser
            using (var parser = new TextFieldParser(new MemoryStream(_csv_data)))
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
