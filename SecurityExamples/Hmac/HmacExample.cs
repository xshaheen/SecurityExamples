using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SecurityExamples.Hmac
{
    public class HmacExample
    {
        public static void Execute()
        {
            const string dataFile = "text.txt";
            const string signedFile = "signedFile.enc";

            // if file not exist create one
            if (!File.Exists(dataFile))
            {
                // Create a file to write to.
                using var sw = File.CreateText(dataFile);

                sw.WriteLine("Here is a message to sign");
            }

            try
            {
                // Create a random key using a random number generator. This would be the
                //  secret key shared by sender and receiver.
                var secretKey = GenerateKey(64);

                // Use the secret key to sign the message file.
                SignFile(secretKey, dataFile, signedFile);

                // Verify the signed file
                VerifyFile(secretKey, signedFile);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error: File not found: {e}");
            }
        }

        /// <summary>
        ///     Computes a keyed hash for a source file and creates a target file with the keyed hash
        ///     prepended to the contents of the source file.
        /// </summary>
        public static void SignFile(byte[] key, string sourceFile, string destFile)
        {
            // Initialize the keyed hash object.
            using var hmac = new HMACSHA512(key);

            using var inStream = new FileStream(sourceFile, FileMode.Open);
            using var outStream = new FileStream(destFile, FileMode.Create);

            // Compute the hash of the input file.
            var hashValue = hmac.ComputeHash(inStream);

            // Write the computed hash value to the output file.
            outStream.Write(hashValue, 0, hashValue.Length);

            // Copy the contents of the sourceFile to the destFile.
            // Reset inStream to the beginning of the file.
            inStream.Position = 0;
            // read 1K at a time
            var buffer = new byte[1024];
            int bytesRead;
            do
            {
                bytesRead = inStream.Read(buffer, 0, 1024);
                outStream.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);
        }

        /// <summary>
        ///     Compares the key in the source file with a new key created for the data portion of the file.
        ///     If the keys compare the data has not been tampered with.
        /// </summary>
        public static bool VerifyFile(byte[] key, string sourceFile)
        {
            var err = false;
            // Initialize the keyed hash object.
            using (var hmac = new HMACSHA512(key))
            {
                // Create an array to hold the keyed hash value read from the file.
                var storedHash = new byte[hmac.HashSize / 8];

                // Create a FileStream for the source file.
                using var inStream = new FileStream(sourceFile, FileMode.Open);

                // Read in the storedHash.
                inStream.Read(storedHash, 0, storedHash.Length);

                // Compute the hash of the remaining contents of the file. The stream is
                // properly positioned at the beginning of the content, immediately after
                // the stored hash value.
                var computedHash = hmac.ComputeHash(inStream);

                // compare the computed hash with the stored value
                if (storedHash.Where((storedBye, i) => computedHash[i] != storedBye).Any())
                    err = true;
            }

            if (err)
            {
                Console.WriteLine("Hash values differ! Signed file has been tampered with!");
                return false;
            }

            Console.WriteLine("Hash values agree -- no tampering occurred.");
            return true;
        }

        // RNGCryptoServiceProvider is an implementation of a random number generator.
        private static byte[] GenerateKey(int length)
        {
            var randomNumber = new byte[length];

            using var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(randomNumber);

            return randomNumber;
        }
    }
}
