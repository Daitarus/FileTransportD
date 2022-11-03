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
            this.answer = answer;            
            this.sessionId = sessionId;
        }

        public override byte[] ToBytes()
        {
            byte[] payload = new byte[2 + sessionId.Length];

            payload[0] = typeCom;
            payload[1] = Convert.ToByte(answer);
            Array.Copy(sessionId, 0, payload, 2, sessionId.Length);

            return payload;
        }

        public override bool ExecuteCommand(ref Transport transport, ref ClientInfo clientInfo)
        {
            return answer;
        }

        public static AuthenticationComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length != 1 + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be {1 + LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            Array.Copy(payload, 1, sessionId, 0, sessionId.Length);

            bool answer = BitConverter.ToBoolean(payload, 0);

            return new AuthenticationComA(answer, sessionId);
        }
    }
}
