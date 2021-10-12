using System.IO;

namespace HashFields.Data.Tests.Fakes
{
    public class MemoryStreamProvider : IStreamProvider
    {
        private readonly byte[] _data;

        public MemoryStreamProvider(byte[] data)
        {
            _data = data;
        }

        public Stream Get()
        {
            return new MemoryStream(_data);
        }
    }
}
