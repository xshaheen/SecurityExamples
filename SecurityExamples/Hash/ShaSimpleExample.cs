using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityExamples.Hash
{
    public class ShaSimpleExample
    {
        /// <summary>
        ///     Hash is a one-way process to generate a fingerprint of data to securely
        ///     store passwords, or can be used to detect malicious change or corruption
        ///     of data (integrity).
        /// </summary>
        public static void Hash()
        {
            const string msg1 = "Hello, world!";
            const string msg2 = "Hel1o, world!";

            Console.WriteLine(Convert.ToBase64String(ComputeHashSha512(msg1)));
            Console.WriteLine(Convert.ToBase64String(ComputeHashSha512(msg2)));
        }

        private static byte[] ComputeHashSha512(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            using var sha = SHA512.Create();

            return sha.ComputeHash(bytes);
        }
    }
}
