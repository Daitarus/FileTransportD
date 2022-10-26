using CryptL;
using ProtocolCryptographyD;
using System.Text;

namespace CommandsKit
{
    public class AuthenticationComR : Command
    {

        public readonly byte[] hashAuthentication;

        public AuthenticationComR(byte[] hashAuthorization, byte[] sessionId)
        {
            if (hashAuthorization == null)
                throw new ArgumentNullException(nameof(hashAuthorization));
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (hashAuthorization.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(hashAuthorization)} size must be {LengthHash}");
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");


            typeCom = (byte)TypeCommand.AUTHORIZATION_R;
            payload = hashAuthorization;

            this.hashAuthentication = hashAuthorization;
            this.sessionId = sessionId;
        }

        public override byte[] ToBytes()
        {

            byte[] bytes = new byte[1 + hashAuthentication.Length + sessionId.Length];

            bytes[0] = typeCom;

            Array.Copy(hashAuthentication, 0, bytes, 1, hashAuthentication.Length);
            Array.Copy(sessionId, 0, bytes, 1 + hashAuthentication.Length, sessionId.Length);

            return bytes;
        }

        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (!clientInfo.authentication)
                {
                    ExecuteRequest.Authentication(transport, ref clientInfo, sessionId, hashAuthentication);
                }
            }
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
