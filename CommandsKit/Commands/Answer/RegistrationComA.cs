using ProtocolTransport;

namespace CommandsKit
{
    public class RegistrationComA : CommandAnswer
    {
        public readonly bool answer;

        public RegistrationComA(bool answer, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.REGISTRATION_A;
            this.sessionId = sessionId;
            this.answer = answer;
        }

        public override byte[] ToBytes()
        {
            byte[] payload = new byte[2 + sessionId.Length];

            payload[0] = typeCom;
            payload[1] = Convert.ToByte(answer);
            Array.Copy(sessionId, 0, payload, 2, sessionId.Length);

            return payload;
        }

        public override bool ExecuteCommand()
        {
            return answer;
        }

        public static RegistrationComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length != 1 + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be {1 + LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            Array.Copy(payload, 1, sessionId, 0, sessionId.Length);

            bool answer = BitConverter.ToBoolean(payload, 0);

            return new RegistrationComA(answer, sessionId);
        }
    }
}
