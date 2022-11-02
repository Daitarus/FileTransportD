using ProtocolCryptographyD;

namespace CommandsKit
{
    public class FileGetComR : Command
    {
        public readonly int fileId;

        public FileGetComR(int fileId, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.FILE_GET_R;
            this.fileId = fileId;
            this.sessionId = sessionId;

            byte[] fileIdBytes = BitConverter.GetBytes(fileId);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(fileIdBytes);
            }

            payload = new byte[fileIdBytes.Length + sessionId.Length];
            Array.Copy(fileIdBytes, 0, payload, 0, fileIdBytes.Length);
            Array.Copy(sessionId, 0, payload, fileIdBytes.Length, sessionId.Length);
        }

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            bool answer = false;
            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (clientInfo.authentication)
                {
                    answer = ExecuteRequest.FileGet(transport, clientInfo, fileId);
                }
            }

            return answer;
        }

        public static FileGetComR BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length <= LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be more {LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            byte[] fileIdBytes = new byte[payload.Length - LengthHash];

            Array.Copy(payload, 0, fileIdBytes, 0, fileIdBytes.Length);
            Array.Copy(payload, fileIdBytes.Length, sessionId, 0, sessionId.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(fileIdBytes);
            }
            int fileId = BitConverter.ToInt32(fileIdBytes, 0);
            return new FileGetComR(fileId, sessionId);
        }
    }
}
