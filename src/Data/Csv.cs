using System;
using System.IO;
using System.Text;

namespace HashFields.Data
{
    public class Csv : IColumnar, IDisposable
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

        public void Write(Stream destination)
        {
            if (_stream is null)
            {
                throw new InvalidOperationException("Csv stream is null.");
            }
            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            _stream.Seek(0, SeekOrigin.Begin);
            _stream.CopyTo(destination);

            _stream.Seek(0, SeekOrigin.Begin);
            destination.Seek(0, SeekOrigin.Begin);
        }

        public void Dispose()
        {
            _stream?.Close();
            GC.SuppressFinalize(this);
        }
    }
}
