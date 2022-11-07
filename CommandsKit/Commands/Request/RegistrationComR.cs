using ProtocolTransport;
using ServerRepository;
using System.Text;

namespace CommandsKit
{
    public class RegistrationComR : CommandRequest
    {
        public readonly string login;
        public readonly byte[] hashAuthentication;

        public RegistrationComR(string login, byte[] hashAuthentication, byte[] sessionId)
        {
            if (login == null)
                throw new ArgumentNullException(nameof(login));
            if (hashAuthentication == null)
                throw new ArgumentNullException(nameof(hashAuthentication));
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (hashAuthentication.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(hashAuthentication)} size must be {LengthHash}");
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");


            typeCom = (byte)TypeCommand.REGISTRATION_R;
            this.login = login;
            this.hashAuthentication = hashAuthentication;
            this.sessionId = sessionId;            
        }

        public override byte[] ToBytes()
        {
            byte[] payload = new byte[1 + login.Length + hashAuthentication.Length + sessionId.Length];
            byte[] loginBytes = Encoding.UTF8.GetBytes(login);
            payload[0] = typeCom;
            Array.Copy(loginBytes, 0, payload, 1, loginBytes.Length);
            Array.Copy(hashAuthentication, 0, payload, 1 + loginBytes.Length, hashAuthentication.Length);
            Array.Copy(sessionId, 0, payload, 1 + loginBytes.Length + hashAuthentication.Length, sessionId.Length);

            return payload;
        }
        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            RegistrationComA com = new RegistrationComA(false, clientInfo.sessionId);

            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (!clientInfo.authentication)
                {
                    RepositoryClient clientR = new RepositoryClient();
                    Client? client = clientR.SelectForName(login);

                    if (client == null)
                    {
                        client = new Client(hashAuthentication, login);
                        clientR.Add(client);
                        clientR.SaveChange();

                        com = new RegistrationComA(true, clientInfo.sessionId);
                    }
                }
            }
           
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
        }

        public static RegistrationComR BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length <= LengthHash + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be more {LengthHash + LengthHash}");

            byte[] login = new byte[payload.Length - (LengthHash + LengthHash)];
            byte[] sessionId = new byte[LengthHash];
            byte[] hashAuth = new byte[LengthHash];

            Array.Copy(payload, 0, login, 0, login.Length);
            Array.Copy(payload, login.Length, hashAuth, 0, hashAuth.Length);
            Array.Copy(payload, login.Length + hashAuth.Length, sessionId, 0, sessionId.Length);

            return new RegistrationComR(Encoding.UTF8.GetString(login), hashAuth, sessionId);
        }
    }
}
