using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SecurityExamples.Symmetric
{
    public class AesExample
    {
        public static void Execute()
        {
            const string msg = "Hello, world!";
            var key = Program.Rng(32);
            var iv = Program.Rng(16);

            Console.WriteLine($"Plain-text: {msg}");

            var cipherText = Encrypt(key, iv, Encoding.UTF8.GetBytes(msg));
            var cipherText2 = Encrypt(key, iv, msg);
            Console.WriteLine($"Cipher-text: {Convert.ToBase64String(cipherText)}");
            Console.WriteLine($"Cipher-text: {Convert.ToBase64String(cipherText2)}");

            var plainText = Decrypt(key, iv, cipherText);
            var plainText2 = Decrypt(key, iv, cipherText2);
            Console.WriteLine($"Decrypt: {Encoding.UTF8.GetString(plainText)}");
            Console.WriteLine($"Decrypt: {Encoding.UTF8.GetString(plainText2)}");
        }

        public static byte[] Encrypt(byte[] key, byte[] iv, byte[] msg)
        {
            using var aes = new AesManaged { Key = key, IV = iv };
            // create an encryptor to perform stream transformation
            var encryptor = aes.CreateEncryptor();

            // create temporary MemoryStream to store the results
            using var ms = new MemoryStream();

            var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

            cs.Write(msg, 0, msg.Length);

            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        public static byte[] Encrypt(byte[] key, byte[] iv, string msg)
        {
            using var aes = new AesManaged { IV = iv, Key = key };
            // create an encryptor to perform stream transformation

            var encryptor = aes.CreateEncryptor();

            // // create temporary MemoryStream to store the results
            using var ms = new MemoryStream();

            var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);

            // create a StreamWriter to write UTF8 string
            var sw = new StreamWriter(cs);
            sw.Write(msg);

            // flush data
            sw.Flush();
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        public static byte[] Decrypt(byte[] key, byte[] iv, byte[] msg)
        {
            using var aes = new AesManaged { Key = key, IV = iv };
            // create an decryptor to perform stream transformation
            var decryptor = aes.CreateDecryptor();

            // create temporary MemoryStream to store the results
            using var ms = new MemoryStream();

            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(msg, 0, msg.Length);
            }

            return ms.ToArray();
        }
    }
}
