using System.Security.Cryptography;
using System.Text;

namespace CryptL
{
    public static class HashSHA256
    {
        public static int Length { get { return 32; } }
        public static byte[] GetHash(byte[] originalData)
        {
            if (originalData == null || originalData.Length == 0)
                throw new ArgumentNullException(nameof(originalData));

            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(originalData);
            }
        }
        public static byte[] GetHash(string originaData)
        {
            byte[] origonalDataBytes = Encoding.UTF8.GetBytes(originaData);
            return GetHash(origonalDataBytes);
        }
    }
}
