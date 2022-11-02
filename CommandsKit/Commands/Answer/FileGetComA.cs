using ProtocolCryptographyD;

namespace CommandsKit
{
    public class FileGetComA : Command
    {
        public static int MaxLength_Info_Block { get { return MaxLengthData - LengthHash - 4; } }

        public readonly byte numBlock = 0;
        public readonly byte allBlock = 0;
        public readonly byte lengthInfo = 0;
        public readonly byte[] fileInfo;
        public readonly byte[] fileBlock;

        public FileGetComA(byte numBlock, byte allBlock, byte lengthInfo, byte[] fileInfo, byte[] fileBlock, byte[] sessionId)
        {
            if (fileInfo == null)
                throw new ArgumentNullException(nameof(fileInfo));
            if (fileBlock == null)
                throw new ArgumentNullException(nameof(fileBlock));
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (fileInfo.Length != lengthInfo)
                throw new ArgumentOutOfRangeException($"{nameof(fileInfo)} size must be {lengthInfo}");
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.FILE_GET_A;
            this.numBlock = numBlock;
            this.allBlock = allBlock;
            this.lengthInfo = lengthInfo;
            this.fileInfo = fileInfo;
            this.fileBlock = fileBlock;
            this.sessionId = sessionId;

            payload = new byte[3 + fileInfo.Length + fileBlock.Length + sessionId.Length];

            payload[0] = numBlock;
            payload[1] = allBlock;
            payload[2] = lengthInfo;

            Array.Copy(fileInfo, 0, payload, 3, fileInfo.Length);
            Array.Copy(fileBlock, 0, payload, 3 + fileInfo.Length, fileBlock.Length);
            Array.Copy(sessionId, 0, payload, 3 + fileInfo.Length + fileBlock.Length, sessionId.Length);
        }

        public override bool ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            ExecuteAnswer.FileGet(clientInfo, numBlock, fileInfo, fileBlock);
            return true;
        }

        public static FileGetComA BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));

            byte numBlock = payload[0];
            byte allBlock = payload[1];
            byte lengthInfo = payload[2];

            byte[] fileInfo = new byte[lengthInfo];
            byte[] fileBlock = new byte[payload.Length - 3 - fileInfo.Length - LengthHash];
            byte[] sessionId = new byte[LengthHash];

            Array.Copy(payload, 3, fileInfo, 0, fileInfo.Length);
            Array.Copy(payload, 3 + fileInfo.Length, fileBlock, 0, fileBlock.Length);
            Array.Copy(payload, 3 + fileInfo.Length + fileBlock.Length, sessionId, 0, sessionId.Length);

            return new FileGetComA(numBlock, allBlock, lengthInfo, fileInfo, fileBlock, sessionId);
        }
    }
}
