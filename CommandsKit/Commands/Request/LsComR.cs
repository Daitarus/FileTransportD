using ProtocolCryptographyD;

namespace CommandsKit
{
    public class LsComR : Command
    {
        public readonly byte[] args = new byte[0];

        public LsComR(byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.LS_R;
            this.sessionId = sessionId;

            payload = sessionId;
        }
        public LsComR(byte[] args, byte[] sessionId)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.LS_R;
            this.args = args;
            this.sessionId = sessionId;

            payload = new byte[args.Length + sessionId.Length];
            Array.Copy(args, 0, payload, 0, args.Length);
            Array.Copy(sessionId, 0, payload, args.Length, sessionId.Length);
        }

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            string lsInfo = "";

            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (clientInfo.authentication)
                {
                    lsInfo = ExecuteRequest.Ls(clientInfo);
                }
            }

            LsComA com = new LsComA(lsInfo.ToString(), clientInfo.sessionId);
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));

            return (lsInfo.Length > 0);
        }

        public static LsComR BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length < LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must or equal be {LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            byte[] args = new byte[0];
            if (payload.Length - LengthHash > 0)
            {
                args = new byte[payload.Length - LengthHash];
                Array.Copy(payload, 0, args, 0, args.Length);
            }
            Array.Copy(payload, args.Length, sessionId, 0, sessionId.Length);

            if (args.Length != 0)
            {
                return new LsComR(args, sessionId);
            }
            else
            {
                return new LsComR(sessionId);
            }
        }
    }
}
