using ProtocolCryptographyD;
using System.Text;

namespace CommandsKit
{
    public class RegistrationComR : Command
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

            payload = new byte[login.Length + hashAuthentication.Length + sessionId.Length];
            byte[] loginBytes = Encoding.UTF8.GetBytes(login);
            Array.Copy(loginBytes, 0, payload, 0, loginBytes.Length);
            Array.Copy(hashAuthentication, 0, payload, loginBytes.Length, hashAuthentication.Length);
            Array.Copy(sessionId, 0, payload, loginBytes.Length + hashAuthentication.Length, sessionId.Length);
        }

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            bool answer = false;
            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (!clientInfo.authentication)
                {
                    answer = ExecuteRequest.Registration(login, hashAuthentication);
                }
            }

            RegistrationComA com = new RegistrationComA(answer, clientInfo.sessionId);
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));

            return answer;
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
