using System.Security.Cryptography;

namespace CryptL
{

    public sealed class CryptRSA : ICrypt
    {
        private RSACryptoServiceProvider rsa;

        private static int allKeyStandardLength = 2324;
        private static int publicKeyStandardLength = 532;

        public byte[] AllKey
        {
            get
            {
                return rsa.ExportCspBlob(true);
            }
            set
            {
                rsa.ImportCspBlob(value);
            }
        }
        public byte[] PublicKey
        {
            get
            {
                return rsa.ExportCspBlob(false);
            }
            set
            {
                rsa.ImportCspBlob(value);
            }
        }

        public CryptRSA()
        {
            rsa = new RSACryptoServiceProvider(4096);
        }
        public CryptRSA(byte[] key, bool ifAllKey)
        {
            CheckExeptionKey(key, ifAllKey);

            rsa = new RSACryptoServiceProvider(4096);
            if (ifAllKey)
            {
                AllKey = key;
            }
            else
            {
                PublicKey = key;
            }
        }

        public byte[] Encrypt(byte[] originalData)
        {
            if (originalData == null || originalData.Length == 0)
                throw new ArgumentNullException(nameof(originalData));

            return rsa.Encrypt(originalData, false);
        }
        public byte[] Decrypt(byte[] encryptData)
        {
            if (encryptData == null || encryptData.Length == 0)
                throw new ArgumentNullException(nameof(encryptData));

            return rsa.Decrypt(encryptData, false);
        }

        public static void CheckExeptionKey(byte[] key, bool ifAllKey)
        {
            int keyStandardLength = 0;

            if (ifAllKey)
            {
                keyStandardLength = allKeyStandardLength;
            }
            else
            {
                keyStandardLength = publicKeyStandardLength;
            }

            if (key == null || key.Length != publicKeyStandardLength)
                throw new Exception($"{nameof(key)} size must be {keyStandardLength}");
        }
    }

}
