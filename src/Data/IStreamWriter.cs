using System;
using System.IO;

namespace HashFields.Data
{
    public interface IStreamWriter : IDisposable
    {
        void Write(Stream destination);
    }
}
