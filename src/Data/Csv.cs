using System;
using System.IO;
using System.Text;

namespace HashFields.Data
{
    public class Csv : IColumnar
    {
        private readonly Stream _stream;

        public Columnar Columnar { get; }

        public Csv(Stream stream)
        {
            _stream = stream;
            Columnar = new Columnar(_stream);
        }

        public Csv(string csvText)
        {
            var text = (csvText ?? String.Empty).Trim();
            var data = Encoding.UTF8.GetBytes(text);

            _stream = new MemoryStream(data);
            Columnar = new Columnar(_stream);
        }
    }
}
