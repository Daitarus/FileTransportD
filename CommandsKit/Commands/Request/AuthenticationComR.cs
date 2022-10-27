using ProtocolCryptographyD;

namespace CommandsKit
{
    public class AuthenticationComR : Command
    {

        public readonly byte[] hashAuthentication;

        public AuthenticationComR(byte[] hashAuthentication, byte[] sessionId)
        {
            if (hashAuthentication == null)
                throw new ArgumentNullException(nameof(hashAuthentication));
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (hashAuthentication.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(hashAuthentication)} size must be {LengthHash}");
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");


            typeCom = (byte)TypeCommand.AUTHORIZATION_R;
            this.hashAuthentication = hashAuthentication;
            this.sessionId = sessionId;

            payload = new byte[hashAuthentication.Length + sessionId.Length];
            Array.Copy(hashAuthentication, 0, payload, 0, hashAuthentication.Length);
            Array.Copy(sessionId, 0, payload, hashAuthentication.Length, sessionId.Length);
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
            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (!clientInfo.authentication)
                {
                    return ExecuteRequest.Authentication(transport, ref clientInfo, sessionId, hashAuthentication);
                }
            }

            return false;
        }

        public static AuthenticationComR BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length != LengthHash + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be {LengthHash + LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            byte[] hashAuth = new byte[LengthHash];

            Array.Copy(payload, 0, hashAuth, 0, hashAuth.Length);
            Array.Copy(payload, hashAuth.Length, sessionId, 0, sessionId.Length);
            return new AuthenticationComR(hashAuth, sessionId);
        }
    }
}
