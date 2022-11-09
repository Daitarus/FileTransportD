using ProtocolTransport;
using ServerRepository;
using System.Text;

namespace CommandsKit
{
    public class FileGetComR : CommandRequest
    {
        public readonly uint fileId;

        public FileGetComR(uint fileId, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.FILE_GET_R;
            this.fileId = fileId;
            this.sessionId = sessionId;
        }

        public override byte[] ToBytes()
        {
            byte[] fileIdBytes = BitConverter.GetBytes(fileId);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(fileIdBytes);
            }

            byte[] payload = new byte[1 + fileIdBytes.Length + sessionId.Length];
            payload[0] = typeCom;
            Array.Copy(fileIdBytes, 0, payload, 1, fileIdBytes.Length);
            Array.Copy(sessionId, 0, payload, 1 + fileIdBytes.Length, sessionId.Length);

            return payload;
        }
        public override void ExecuteCommand(Transport transport, ref ClientInfo clientInfo)
        {
            bool answer = false;
            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (clientInfo.authentication)
                {
                    RepositoryClientFile clientFileR = new RepositoryClientFile();
                    List<uint> filesId = clientFileR.IdFileForClient(clientInfo.clientId);

                    foreach (uint id in filesId)
                    {
                        if (id == fileId)
                        {
                            RepositoryFile fileR = new RepositoryFile();
                            ServerRepository.File file = fileR.SelectId(fileId);
                            if (file != null)
                            {
                                FileInfo fileInfo = new FileInfo(file.FullPath);
                                byte[] fileInfoBytes = Encoding.UTF8.GetBytes(file.Name);
                                long MaxLengthBlock = FileGetComA.MaxLength_Info_Block - fileInfoBytes.Length;

                                if (fileInfo.Exists)
                                {
                                    if (fileInfo.Length <= (MaxLengthData * 255))
                                    {
                                        int numAllBlock = (int)Math.Ceiling((double)fileInfo.Length / (double)MaxLengthBlock);
                                        if ((fileInfo.Length > 0) && (numAllBlock < 256))
                                        {
                                            using (FileStream fstream = System.IO.File.Open(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                            {
                                                for (byte i = 0; i < numAllBlock; i++)
                                                {
                                                    byte[] buffer = new byte[MaxLengthBlock];
                                                    long beginRead = i * MaxLengthBlock;
                                                    fstream.Seek(beginRead, SeekOrigin.Begin);
                                                    int numReadByte = fstream.Read(buffer);
                                                    byte[] bufferFile = new byte[numReadByte];
                                                    Array.Copy(buffer, 0, bufferFile, 0, numReadByte);

                                                    Command com = new FileGetComA(i, (byte)numAllBlock, (byte)fileInfoBytes.Length, fileInfoBytes, bufferFile, clientInfo.sessionId);
                                                    transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
                                                }
                                            }
                                            answer = true;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }

            if (!answer)
            {
                Command com = new FileGetComA(0, 1, 0, new byte[0], new byte[0], clientInfo.sessionId);
                transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
            }
        }

        public static FileGetComR BytesToCom(byte[] payload)
        {
            if (payload == null)
                throw new ArgumentNullException(nameof(payload));
            if (payload.Length < 4 + LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(payload)} size must be more {4 + LengthHash}");

            byte[] sessionId = new byte[LengthHash];
            byte[] fileIdBytes = new byte[payload.Length - LengthHash];

            Array.Copy(payload, 0, fileIdBytes, 0, fileIdBytes.Length);
            Array.Copy(payload, fileIdBytes.Length, sessionId, 0, sessionId.Length);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(fileIdBytes);
            }
            uint fileId = BitConverter.ToUInt32(fileIdBytes, 0);
            return new FileGetComR(fileId, sessionId);
        }
    }
}
