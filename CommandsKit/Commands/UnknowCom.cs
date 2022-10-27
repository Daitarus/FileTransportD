using CryptL;
using ProtocolCryptographyD;

namespace CommandsKit
{
    public class UnknowCom : Command
    {
        public readonly byte[] bytes = new byte[0];
        public UnknowCom(byte[]? bytes, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.UNKNOW;
            if(bytes != null)
            {
                this.bytes = bytes;
            }
            this.sessionId = sessionId;

            if (bytes != null)
            {
                payload = new byte[bytes.Length + sessionId.Length];
                Array.Copy(bytes, 0, payload, 0, bytes.Length);
                Array.Copy(sessionId, 0, payload, bytes.Length, sessionId.Length);
            }
            else
            {
                payload = sessionId;
            }

        }
        public static UnknowCom BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length < LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be more or equaly {LengthHash})");

            byte[] bytes = new byte[payload.Length - LengthHash];
            Array.Copy(payload, 0, bytes, 0, bytes.Length);

            byte[] sessionId = new byte[LengthHash];
            Array.Copy(payload, bytes.Length, sessionId, 0, sessionId.Length);

            return new UnknowCom(bytes, sessionId);
        }
    }
}
