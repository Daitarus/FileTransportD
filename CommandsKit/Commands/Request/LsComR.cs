using ProtocolTransport;
using ServerRepository;
using System.Text;

namespace CommandsKit
{
    public class LsComR : CommandRequest
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
        }

        public override byte[] ToBytes()
        {
            byte[] payload = new byte[1 + args.Length + sessionId.Length];
            payload[0] = typeCom;
            Array.Copy(args, 0, payload, 1, args.Length);
            Array.Copy(sessionId, 0, payload, 1 + args.Length, sessionId.Length);

            return payload;
        }
        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            string lsInfo = "";

            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (clientInfo.authentication)
                {
                    RepositoryClientFile clientFileR = new RepositoryClientFile();
                    List<uint> allFileId = clientFileR.IdFileForClient(clientInfo.clientId);

                    if (allFileId.Count > 0)
                    {
                        RepositoryFile fileR = new RepositoryFile();
                        foreach (uint id in allFileId)
                        {
                            ServerRepository.File? file = fileR.SelectId(id);
                            if (file != null)
                            {
                                lsInfo += String.Format("Id:{0} Directory:{1}\n", file.Id, file.Path);
                            }
                        }
                    }
                }
            }

            LsComA com = new LsComA(lsInfo, clientInfo.sessionId);
            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
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
