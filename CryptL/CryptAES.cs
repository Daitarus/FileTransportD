using System.Security.Cryptography;

namespace CryptL
{
    public sealed class CryptAES : ICrypt
    {

        private Aes aes;

        private static int keyStandardLength = 32;
        private static int ivStandardLength = 16;
        public byte[] Key { get { return aes.Key; } }
        public byte[] IV { get { return aes.IV; } }

        public CryptAES() 
        {
            aes = Aes.Create();
        }
        public CryptAES(byte[] unionKeyIv)
        {
            byte[] key, iv;
            PartitionKeyIV(unionKeyIv, out key, out iv);

            aes = Aes.Create();

            aes.Key = key;
            aes.IV = iv;
        }
        public CryptAES(byte[] key, byte[] iv)
        {
            aes = Aes.Create();

            if (key == null || key.Length != keyStandardLength)
                throw new Exception($"AES {nameof(key)} must be {keyStandardLength} bytes");
            if (iv == null || iv.Length != ivStandardLength)
                throw new Exception($"AES {nameof(iv)} must be {ivStandardLength} bytes");

            aes.Key = key;
            aes.IV = iv;
        }

        public byte[] Encrypt(byte[] originalData)
        {
            if (originalData == null || originalData.Length == 0)
                throw new ArgumentNullException(nameof(originalData));

            byte[] encryptData = aes.EncryptCbc(originalData, aes.IV);
            return encryptData;
        }

        public byte[] Decrypt(byte[] encryptData)
        {
            if (encryptData == null || encryptData.Length == 0)
                throw new ArgumentNullException(nameof(encryptData));

            byte[] decryptData =  aes.DecryptCbc(encryptData, aes.IV);

            return decryptData;
        }

        public byte[] UnionKeyIV()
        {
            byte[] result = new byte[keyStandardLength + ivStandardLength];
            Array.Copy(aes.Key,0,result,0,keyStandardLength);
            Array.Copy(aes.IV,0,result,keyStandardLength,ivStandardLength);

            return result;
        }

        public void PartitionKeyIV(byte[] keyIV, out byte[] key, out byte[] iv)
        {
            CheckExeptionUnionKey(keyIV);

            key = new byte[keyStandardLength];
            iv = new byte[ivStandardLength];

            Array.Copy(keyIV, 0, key,0, keyStandardLength);
            Array.Copy(keyIV, keyStandardLength, iv, 0, ivStandardLength);
        }

        public static void CheckExeptionUnionKey(byte[] keyIV)
        {
            if (keyIV == null)
                throw new ArgumentNullException(nameof(keyIV));

            if (keyIV.Length != keyStandardLength + ivStandardLength)
                throw new ArgumentException($"{nameof(keyIV)} size not equal {keyStandardLength + ivStandardLength}");

        }
    }
}
