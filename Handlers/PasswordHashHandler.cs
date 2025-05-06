using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace GameBackEnd.Handlers
{
    public class PasswordHashHandler
    {
        private static int _iterarionCount = 100000;
        private static RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

        public static string HashPassword(string password)
        {
            int saltSize = 128 / 8;
            var salt = new byte[saltSize];
            _randomNumberGenerator.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, _iterarionCount, 256 / 8);
            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01;
            WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
            WriteNetworkByteOrder(outputBytes, 5, (uint)_iterarionCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint)saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);

            return Convert.ToBase64String(outputBytes);
        }
        public static bool VerifyPassword(string password, string hash)
        {
            try
            {
                var hashedPassword = Convert.FromBase64String(hash);

                if (hashedPassword.Length < 13 || hashedPassword[0] != 0x01)
                    return false;

                var prf = (KeyDerivationPrf)ReadNetworkByteOrder(hashedPassword, 1);
                var iterationCount = (int)ReadNetworkByteOrder(hashedPassword, 5);
                var saltLength = (int)ReadNetworkByteOrder(hashedPassword, 9);

                if (prf != KeyDerivationPrf.HMACSHA512)
                    return false;

                if (saltLength != 16) 
                    return false;

                int expectedLength = 13 + saltLength + 32;
                if (hashedPassword.Length != expectedLength)
                    return false;

                var salt = new byte[saltLength];
                Buffer.BlockCopy(hashedPassword, 13, salt, 0, saltLength);

                var storedSubkey = new byte[32];
                Buffer.BlockCopy(hashedPassword, 13 + saltLength, storedSubkey, 0, 32);

                var derivedSubkey = KeyDerivation.Pbkdf2(
                    password,
                    salt,
                    prf,
                    iterationCount,
                    32
                );

                bool matches = true;
                for (int i = 0; i < storedSubkey.Length; i++)
                {
                    if (storedSubkey[i] != derivedSubkey[i])
                    {
                        matches = false;
                        break;
                    }
                }

                return matches;
            }
            catch
            {
                return false;
            }
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return (uint)(
                buffer[offset] << 24 |
                buffer[offset + 1] << 16 |
                buffer[offset + 2] << 8 |
                buffer[offset + 3]
            );
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }
    }
}

