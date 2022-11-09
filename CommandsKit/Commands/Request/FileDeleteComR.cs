using ProtocolTransport;
using ServerRepository;

namespace CommandsKit
{
    public class FileDeleteComR : CommandRequest
    {
        public readonly uint fileId;

        public FileDeleteComR(uint fileId, byte[] sessionId)
        {
            if (sessionId == null)
                throw new ArgumentNullException(nameof(sessionId));
            if (sessionId.Length != LengthHash)
                throw new ArgumentOutOfRangeException($"{nameof(sessionId)} size must be {LengthHash}");

            typeCom = (byte)TypeCommand.FILE_DELETE_R;
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
            Command com = new FileDeleteComA(false, clientInfo.sessionId);

            if (Enumerable.SequenceEqual(clientInfo.sessionId, sessionId))
            {
                if (clientInfo.authentication)
                {
                    RepositoryClientFile clientFileR = new RepositoryClientFile();
                    List<Client_File> clientFiles = clientFileR.SelectFileId(fileId);
                    
                    foreach (Client_File clientFile in clientFiles)
                    {
                        if (clientInfo.clientId == clientFile.Id_Client)
                        {
                            clientFiles.Remove(clientFile);
                            clientFileR.Remove(clientFile);
                            clientFileR.SaveChange();

                            if (clientFiles.Count() == 0)
                            {
                                RepositoryFile fileR = new RepositoryFile();
                                ServerRepository.File file = fileR.SelectId(fileId);
                                if (file != null)
                                {
                                    fileR.Remove(file);
                                    fileR.SaveChange();

                                    FileInfo fileInfo = new FileInfo(file.FullPath);
                                    if (fileInfo.Exists)
                                    {
                                        fileInfo.Delete();
                                    }
                                }
                            }

                            com = new FileDeleteComA(true, clientInfo.sessionId);
                            break;
                        }
                    }
                }
            }

            transport.SendData(clientInfo.aes.Encrypt(com.ToBytes()));
        }

        public static FileDeleteComR BytesToCom(byte[] payload)
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
            return new FileDeleteComR(fileId, sessionId);
        }
    }
}
