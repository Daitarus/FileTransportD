using CryptL;
using ProtocolCryptographyD;

namespace CommandsKit
{
    public class AuthenticationComA : Command
    {
        public readonly bool answer;

        public AuthenticationComA(bool answer, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.AUTHORIZATION_A;
            payload = new byte[1];
            this.sessionId = sessionId;

            if(answer)
            {
                payload[0] = 1;
            }
            else
            {
                payload[0] = 0;
            }

            this.answer = answer;
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[2 + sessionId.Length];

            bytes[0] = typeCom;
            bytes[1] = payload[0];
            Array.Copy(sessionId, 0, bytes, 2, sessionId.Length);

            return bytes;
        }

        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            ExecuteAnswer.Authentication(transport, ref clientInfo, answer);
        }

        public static AuthenticationComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length != 1 + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be {1 + LengthHash}");

            byte[] sessionId = new byte[HashSHA256.Length];
            Array.Copy(payload, 1, sessionId, 0, sessionId.Length);

            bool answer = false;
            if(payload[0]==1)
            {
                answer = true;
            }

            return new AuthenticationComA(answer, sessionId);
        }
    }
}
