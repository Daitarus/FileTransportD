using ProtocolCryptographyD;
using System.Text;

namespace CommandsKit
{
    internal static class ExecuteAnswer
    {
        public static string pathWriteFile = "";
        public static void Ls(string lsInfo)
        {
            Console.Write("\n{0}\n", lsInfo);
        }
        public static void FileGet(ClientInfo clientInfo, int numBlock, byte[] fileInfoBytes, byte[] fileBlock)
        {
            StringBuilder fileInfoStr = new StringBuilder(pathWriteFile);
            fileInfoStr.Append(Encoding.UTF8.GetString(fileInfoBytes));
            FileInfo fileInfo = new FileInfo(fileInfoStr.ToString());

            FileMode fmode = FileMode.Append;
            if (numBlock == 0)
            {
                fmode = FileMode.Create;
            }

            using (FileStream fstream = new FileStream(fileInfo.FullName, fmode, FileAccess.Write, FileShare.ReadWrite))
            {
                fstream.Write(fileBlock);
            }
        }
    }
}
