using CryptL;
using ProtocolCryptographyD;

namespace CommandsKit
{
    public class AuthorizationComA : ICommand
    {
        private TypeCom type;
        private byte[] payLoad;

        private byte lengthAnswer = 1;
        public readonly bool answer;
        public AuthorizationComA(bool answer)
        {
            type = TypeCom.AUTHORIZATION_A;
            payLoad = new byte[1];
            if(answer)
            {
                payLoad[0] = 1;
            }
            else
            {
                payLoad[0] = 0;
            }

            this.answer = answer;
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
            byte[] bytes = new byte[lengthAnswer + 1];

            bytes[0] = (byte)type;
            bytes[1] = payLoad[0];

            return bytes;
        }

        public void ExecuteAction(Transport transport, CryptAES aes)
        {

        }

        public static AuthorizationComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(answer));
            if (payload.Length != 1)
                throw new ArgumentException($"{nameof(answer)} size must be 1");

            if(payload[0]==1)
            {
                return new AuthorizationComA(true);
            }
            else
            {
                return new AuthorizationComA(false);
            }
        }
    }
}
