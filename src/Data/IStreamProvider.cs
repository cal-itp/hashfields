using System.IO;

namespace HashFields.Data
{
    public interface IStreamProvider
    {
        Stream Get();
    }
}
