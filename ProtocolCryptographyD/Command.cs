using CryptL;

namespace ProtocolCryptographyD
{
    public abstract class Command
    {
        public static int MaxLengthData { get { return 16777199; } }
        public static int LengthHash {  get { return HashSHA256.Length; } }

        protected byte typeCom;
        protected byte[] payload = new byte[0];
        protected byte[] sessionId = new byte[0];

        public byte TypeCom { get { return typeCom; } }
        public byte[] Payload { get { return payload; } }
        public byte[] SessionId { get { return sessionId; } }

        public virtual byte[] ToBytes() 
        {
            byte[] bytes = new byte[1 + payload.Length];

            bytes[0] = typeCom;
            Array.Copy(payload, 0, bytes, 1, payload.Length);

            return bytes;
        }
        public virtual bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            return false;
        }
    }
}
