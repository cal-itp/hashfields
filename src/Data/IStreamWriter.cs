using System.IO;

namespace HashFields.Data
{
    public interface IStreamWriter
    {
        void Write(Stream destination);
    }
}
