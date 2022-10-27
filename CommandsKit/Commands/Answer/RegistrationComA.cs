using ProtocolCryptographyD;

namespace CommandsKit
{
    public class RegistrationComA : Command
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

            payload = new byte[1 + sessionId.Length];

            if (answer)
            {
                payload[0] = 1;
            }
            else
            {
                payload[0] = 0;
            }
            Array.Copy(sessionId, 0, payload, 1, sessionId.Length);
        }

        public override byte[] ToBytes()
        {
            byte[] bytes = new byte[1 + payload.Length];

            bytes[0] = typeCom;
            Array.Copy(payload, 0, bytes, 1, payload.Length);

            return bytes;
        }

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            return ExecuteAnswer.Registration(transport, ref clientInfo, answer);
        }

        public static RegistrationComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length != 1 + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be {1 + LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            Array.Copy(payload, 1, sessionId, 0, sessionId.Length);

            bool answer = false;
            if (payload[0] == 1)
            {
                answer = true;
            }

            return new RegistrationComA(answer, sessionId);
        }
    }
}
