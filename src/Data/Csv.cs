using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.VisualBasic.FileIO;
using System.Text;

namespace HashFields.Data
{
    public class Csv
    {
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly byte[] _csv_data;
        private readonly string _csv_text;

        public Csv(string csv_text)
        {
            _csv_text = csv_text ?? String.Empty;
            _csv_data = _encoding.GetBytes(_csv_text);
        }

        public IDictionary<string, List<string>> ToColumnar()
        {
            var columnar = new Dictionary<string, List<string>>();

            if (String.IsNullOrEmpty(_csv_text))
            {
                return columnar;
            }

            return columnar;
        }
    }
}
