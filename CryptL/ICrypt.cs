namespace CryptL
{
    public interface ICrypt
    {
        public abstract byte[] Encrypt(byte[] originalData);
        public abstract byte[] Decrypt(byte[] encryptData);        
    }
}
