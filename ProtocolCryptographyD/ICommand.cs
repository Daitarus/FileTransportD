using CryptL;

namespace ProtocolCryptographyD
{
    public interface ICommand
    {
        static int MaxLengthData { get { return 16777199; } }

        public byte GetTypeCom();
        public byte[] GetPayLoad();
        public byte[] ToBytes();
        public void ExecuteAction(Transport transport, CryptAES aes);
    }
}
