using CryptL;

namespace ProtocolCryptographyD
{
    public abstract class Command
    {
        public static int MaxLengthData { get { return 16777199; } }
        public static int LengthHash {  get { return HashSHA256.Length; } }

        protected byte typeCom;
        protected byte[] sessionId = new byte[0];

        public byte TypeCom { get { return typeCom; } }
        public byte[] SessionId { get { return sessionId; } }

        public abstract byte[] ToBytes();
        public abstract bool ExecuteCommand(ref Transport transport, ref ClientInfo clientInfo);
    }
}
