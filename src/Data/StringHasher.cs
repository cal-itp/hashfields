using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HashFields.Data
{
    public class StringHasher
    {
        public static readonly string[] SupportedAlgorithms =
        {
            "SHA256",
            "SHA384",
            "SHA512"
        };

        private readonly string _algorithm;

        public StringHasher() : this(SupportedAlgorithms.Last())
        {
        }

        public StringHasher(string hashAlgorithm)
        {
            if (SupportedAlgorithms.Contains(hashAlgorithm, StringComparer.OrdinalIgnoreCase))
            {
                _algorithm = hashAlgorithm;
            }
            else
            {
                throw new ArgumentException("hashAlgorithm is unsupported");
            }
        }

        public string Hash(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);

            using var algo = GetAlgorithm();
            var hashed = algo.ComputeHash(data);

            return BitConverter.ToString(hashed);
        }

        private HashAlgorithm GetAlgorithm()
        {
            return HashAlgorithm.Create(_algorithm);
        }
    }
}
