using ProtocolCryptographyD;

namespace ProtocolCryptographyD
{
    public interface IParser
    {
        public Command Parse(byte[] buffer);
    }
}
