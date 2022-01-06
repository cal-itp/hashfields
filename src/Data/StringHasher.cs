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

        public string Hash(string input, bool hyphens = true, bool lowercase = false)
        {
            var data = Encoding.UTF8.GetBytes(input);

            using var algo = GetAlgorithm();
            var hashed = BitConverter.ToString(algo.ComputeHash(data));

            if (!hyphens)
            {
                hashed = hashed.Replace("-", "");
            }
            if (lowercase)
            {
                hashed = hashed.ToLower();
            }

            return hashed;
        }

        private HashAlgorithm GetAlgorithm()
        {
            return HashAlgorithm.Create(_algorithm);
        }
    }
}
