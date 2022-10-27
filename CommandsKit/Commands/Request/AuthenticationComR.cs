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

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            bool answer = false;
            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (!clientInfo.authentication)
                {
                    answer = ExecuteRequest.Authentication(ref clientInfo, hashAuthentication);
                }
            }

            RegistrationComA com = new RegistrationComA(answer, clientInfo.sessionId);
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));

            return answer;
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
