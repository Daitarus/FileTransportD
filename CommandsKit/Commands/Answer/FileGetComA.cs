using ConsoleWorker;
using ProtocolTransport;
using System.Text;

namespace CommandsKit
{
    public class FileGetComA : CommandAnswer
    {
        public static long MaxLength_Info_Block { get { return MaxLengthData - LengthHash - 4; } }
        private static string path = "";
        public static string Path
        {
            set
            {
                if (value != null && value.Length > 0)
                {
                    if (value.Length - 1 != '\\')
                    {
                        value += '\\';
                    }
                    path = value;
                }
                else
                {
                    path = "";
                }
            }
            get { return path; }
        }

        public readonly byte numBlock;
        public readonly byte allBlock;
        public readonly byte lengthInfo;
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
        }

        public override byte[] ToBytes()
        {
            byte[] payload = new byte[4 + fileInfo.Length + fileBlock.Length + sessionId.Length];

            payload[0] = typeCom;
            payload[1] = numBlock;
            payload[2] = allBlock;
            payload[3] = lengthInfo;

            Array.Copy(fileInfo, 0, payload, 4, fileInfo.Length);
            Array.Copy(fileBlock, 0, payload, 4 + fileInfo.Length, fileBlock.Length);
            Array.Copy(sessionId, 0, payload, 4 + fileInfo.Length + fileBlock.Length, sessionId.Length);

            return payload;
        }
        public override bool ExecuteCommand()
        {
            StringBuilder fileInfoStr = new StringBuilder(path);
            fileInfoStr.Append(Encoding.UTF8.GetString(this.fileInfo));
            FileInfo fileInfo = new FileInfo(fileInfoStr.ToString());

            if(!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }

            FileMode fmode = FileMode.Append;
            if (numBlock == 0)
            {
                fmode = FileMode.Create;
            }

            using (FileStream fstream = new FileStream(fileInfo.FullName, fmode, FileAccess.Write, FileShare.ReadWrite))
            {
                fstream.Write(fileBlock);
            }

            PrintMessage.PrintColorMessage(CreatorOutString.GetLoadString(String.Format("Download \"{0}\"", fileInfoStr), numBlock, allBlock), ConsoleColor.White);
            if (numBlock + 1 == allBlock)
            {
                Console.WriteLine();
            }

            return (numBlock + 1 < allBlock);
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
