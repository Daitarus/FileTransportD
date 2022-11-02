using ProtocolCryptographyD;
using System.Text;

namespace CommandsKit
{
    public class LsComA : Command
    {
        public readonly string lsInfo;
        public LsComA(string lsInfo, byte[] sessionId)
        {
            if(lsInfo == null)
                throw new ArgumentNullException(nameof(lsInfo));
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.LS_A;
            this.lsInfo = lsInfo;
            this.sessionId = sessionId;

            byte[] lsInfoBytes = new byte[0];
            
            if (lsInfo.Length > 0)
            {
                lsInfoBytes = Encoding.UTF8.GetBytes(lsInfo);
                payload = new byte[lsInfoBytes.Length + sessionId.Length];
                Array.Copy(lsInfoBytes, 0, payload, 0, lsInfoBytes.Length);
            }
            Array.Copy(sessionId, 0, payload, lsInfoBytes.Length, sessionId.Length);
        }

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            ExecuteAnswer.Ls(lsInfo);
            return true;
        }

        public static LsComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length < LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be more or equal {LengthHash}");

            string lsInfo = "";
            byte[] lsInfoBytes = new byte[0];
            byte[] sessionId = new byte[LengthHash];
            if(payload.Length - sessionId.Length > 0)
            {
                lsInfoBytes = new byte[payload.Length - sessionId.Length];
                Array.Copy(payload, 0, lsInfoBytes, 0, lsInfoBytes.Length);
                lsInfo = Encoding.UTF8.GetString(lsInfoBytes);
            }
            Array.Copy(payload, lsInfoBytes.Length, sessionId, 0, sessionId.Length);

            return new LsComA(lsInfo, sessionId);
        }
    }
}
