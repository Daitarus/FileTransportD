using CryptL;
using ProtocolCryptographyD;
using System.Text;

namespace CommandsKit
{
    public class AuthorizationComR : ICommand
    {
        private TypeCom type;
        private byte[] payLoad;

        private int hashLength = HashSHA256.Length;
        public readonly byte[] hashAuthorization;

        public AuthorizationComR(byte[] hashAuthorization)
        {
            if (hashAuthorization == null)
                throw new ArgumentNullException(nameof(hashAuthorization));
            if(hashAuthorization.Length != hashLength)
                throw new ArgumentException($"{nameof(hashAuthorization)} size must bee {hashLength}");

            type = TypeCom.AUTHORIZATION_R;
            payLoad = hashAuthorization;

            this.hashAuthorization = hashAuthorization;
        }
        public byte GetTypeCom()
        {
            return (byte)type;
        }
        public byte[] GetPayLoad()
        {
            return payLoad;
        }

        public byte[] ToBytes()
        {

            byte[] bytes = new byte[hashAuthorization.Length + 1];

            bytes[0] = (byte)type;

            Array.Copy(hashAuthorization, 0, bytes, 1, hashAuthorization.Length);

            return bytes;
        }

        public void ExecuteAction(Transport transport, CryptAES aes)
        {
            byte[] hash = HashSHA256.GetHash(Encoding.UTF8.GetBytes("123"));
            AuthorizationComA com = new AuthorizationComA(Enumerable.SequenceEqual(hashAuthorization, hash));
            transport.SendData(aes.Encrypt(com.ToBytes()));
        }

        public static AuthorizationComR BytesToCom(byte[] payload)
        {
            return new AuthorizationComR(payload);
        }
    }
}
