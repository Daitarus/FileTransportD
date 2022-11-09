using ProtocolTransport;
using ServerRepository;

namespace CommandsKit
{
    public class AuthenticationComR : CommandRequest
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
        }

        public override byte[] ToBytes()
        {
            byte[] payload = new byte[1 + hashAuthentication.Length + sessionId.Length];
            payload[0] = typeCom;
            Array.Copy(hashAuthentication, 0, payload, 1, hashAuthentication.Length);
            Array.Copy(sessionId, 0, payload, 1 + hashAuthentication.Length, sessionId.Length);

            return payload;
        }

        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            DateTime timeAuthentication = DateTime.Now;

            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (!clientInfo.authentication)
                {
                    RepositoryClient clientR = new RepositoryClient();
                    Client? client = clientR.SelectForHash(hashAuthentication);

                    if (client != null)
                    {
                        clientInfo.authentication = true;
                        clientInfo.clientId = client.Id;

                        RepositoryHistory historyR = new RepositoryHistory();
                        History history = new History(clientInfo.endPoint, timeAuthentication, "Authentication", client.Id);
                        historyR.Add(history);
                        historyR.SaveChange();
                    }
                }
            }

            RegistrationComA com = new RegistrationComA(clientInfo.authentication, clientInfo.sessionId);
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
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
