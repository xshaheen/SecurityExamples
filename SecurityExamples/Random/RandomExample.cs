using System;
using System.Security.Cryptography;

namespace SecurityExamples.Random
{
    public class RandomExample
    {
        /// <summary>
        ///     System.Random is a pseudo-random number generator that will gives the appearance of
        ///     randomness if you provide a different seed value every time. it is fine for simple
        ///     scenarios like simulating a dice roll.
        /// </summary>
        /// <remarks>
        ///     * Shared seed values act as a secret key, so if you use the same random number
        ///     generation algorithm with the same seed value in two applications, then
        ///     they can generate the same "random" sequences of numbers.
        ///     * Problem with System.Random
        ///     - If you provide the same initial seed value, you will get the same numbers out.
        ///     - Not thread-safe
        /// </remarks>
        public static void Random()
        {
            for (var i = 0; i < 10; i++) Console.WriteLine($"{GetRandom(123, -10, 11),3}");

            static int GetRandom(int seed, int min, int max)
            {
                var rnd = new System.Random(seed);

                return rnd.Next(min, max);
            }
        }

        /// <summary>
        ///     RandomNumber Generator derived types such as RNGCryptoServiceProvider provide
        ///     a good quality and non-deterministic random number can used to create symmetric
        ///     and asymmetric encryption keys or IV.
        /// </summary>
        /// <remarks>
        ///     RNGCryptoServiceProvider uses entropy from the following sources:
        ///     – The current running process ID
        ///     – The current thread ID
        ///     – A tick counter from since the time the machine was rebooted
        ///     – The current time
        ///     – High-precision performance counters
        ///     – A hash of user data, such as username, computer name, and so forth
        ///     – Internal high-precision timers
        ///     Trad-off over System.Random
        ///     - is much slower to execute.
        /// </remarks>
        public static void CryptoRandom()
        {
            Console.WriteLine(Convert.ToBase64String(Rng(32)));
        }

        public static byte[] Rng(int length)
        {
            var randomNumber = new byte[length];

            using var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(randomNumber);

            return randomNumber;
        }
    }
}
