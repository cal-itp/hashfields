using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HashFields.Data
{
    /// <summary>
    /// Converts input strings into their hashed, hexadecimal string equivalents using a configurable hashing algorithm.
    /// </summary>
    public class StringHasher
    {
        public static readonly string[] SupportedAlgorithms =
        {
            "SHA256",
            "SHA384",
            "SHA512"
        };

        private readonly string _algorithm;

        /// <summary>
        /// Initialize a new StringHasher using the default hashing algorithm.
        /// </summary>
        public StringHasher() : this(SupportedAlgorithms.Last())
        {
        }

        /// <summary>
        /// Initialize a new StringHasher that uses the given hashing algorithm.
        /// </summary>
        /// <param name="hashAlgorithm">The name of one of the supported hashing algorithms, e.g. "SHA256".</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="hashAlgorithm" /> is not a supported hashing algorithm.</exception>
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

        /// <summary>
        /// Produce a hashed string version of an input string, using this StringHasher's algorithm.
        /// Output hashes are formatted like those from <c>System.BitConverter.ToString(byte[])</c>.
        /// </summary>
        /// <see cref="System.BitConverter.ToString(byte[])" />
        /// <param name="input">The input string to hash.</param>
        /// <param name="hyphens">Whether to produce the output hash with hyphens separating each byte. The default is <c>true</c>.</param>
        /// <param name="lowercase">Whether to produce the output hash using lowercase hexadecimal digits. The default is <c>false</c>.</param>
        /// <returns>A string containing the hashed input string.</returns>
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
