namespace ProtocolCryptographyD
{
    public interface IParser
    {
        public ICommand Parse(byte[] buffer);
    }
}
