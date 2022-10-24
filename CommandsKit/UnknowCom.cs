using CryptL;
using ProtocolCryptographyD;

namespace CommandsKit
{
    public class UnknowCom : ICommand
    {
        private TypeCom type;
        private byte[] payLoad = new byte[0];

        public UnknowCom()
        {
            type = TypeCom.UNKNOW;
            payLoad = new byte[0];
        }
        public UnknowCom(byte[] payLoad)
        {
            type = TypeCom.UNKNOW;
            if (payLoad != null)
            {
                this.payLoad = payLoad;
            }
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
            byte[] bytes = new byte[1];

            if (payLoad != null)
            {
                bytes = new byte[payLoad.Length + 1];
                bytes[0] = (byte)type;
                Array.Copy(payLoad, 0, bytes, 1, payLoad.Length);
            }
            bytes[0] = (byte)type;

            return bytes;
        }
        public void ExecuteAction(Transport transport, CryptAES aes) { }

        public static UnknowCom BytesToCom(byte[] payLoad)
        {
            return new UnknowCom(payLoad);
        }
    }
}
