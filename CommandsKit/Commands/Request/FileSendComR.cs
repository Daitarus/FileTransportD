using ConsoleWorker;
using ProtocolTransport;
using System.Text;
using ServerRepository;

namespace CommandsKit
{
    public class FileSendComR : CommandRequest
    {
        public static long MaxLength_Info_Block { get { return MaxLengthData - LengthHash - 4; } }
        public static string path = @"C:\Users\User\Desktop\DataServer\";

        public readonly byte numBlock;
        public readonly byte allBlock;
        public readonly byte lengthInfo;
        public readonly byte[] fileInfo;
        public readonly byte[] fileBlock;

        public FileSendComR(byte numBlock, byte allBlock, byte lengthInfo, byte[] fileInfo, byte[] fileBlock, byte[] sessionId)
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

            typeCom = (byte)TypeCommand.FILE_SEND_R;
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
        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            Command com = new FileSendComA(false, sessionId);

            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (clientInfo.authentication)
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

                    com = new FileSendComA(true, sessionId);

                    if(numBlock + 1 == allBlock)
                    {
                        RepositoryFile fileR = new RepositoryFile();
                        ServerRepository.File file = new ServerRepository.File(fileInfo.Name, Encoding.UTF8.GetString(this.fileInfo), fileInfo.FullName);
                        fileR.Add(file);
                        fileR.SaveChange();
                        file = fileR.GetToPath(file.Path);

                        RepositoryClientFile clientFileR = new RepositoryClientFile();
                        Client_File clientFile = new Client_File(clientInfo.clientId, file.Id);
                        clientFileR.Add(clientFile);
                        clientFileR.SaveChange();
                    }
                }
            }

            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
        }
        public static FileSendComR BytesToCom(byte[] payload)
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

            return new FileSendComR(numBlock, allBlock, lengthInfo, fileInfo, fileBlock, sessionId);
        }
    }
}
