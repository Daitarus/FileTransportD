using ProtocolCryptographyD;
using System.Text;
using ConsoleWorker;


namespace CommandsKit
{
    internal static class ExecuteAnswer
    {
        public static string pathWriteFile = "";
        public static void Ls(string lsInfo)
        {
            string outStr = String.Format("\n{0}\n", lsInfo);
            PrintMessage.PrintColorMessage(outStr.ToString(), ConsoleColor.White);
        }
        public static void FileGet(ClientInfo clientInfo, int numBlock, int allBlock, byte[] fileInfoBytes, byte[] fileBlock)
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

            string outStr = String.Format("Download \"{0}\"", fileInfoStr);
            PrintMessage.PrintColorMessage(CreatorOutString.GetLoadString(outStr, numBlock, allBlock), ConsoleColor.White);
            if (numBlock + 1 == allBlock)
            {
                Console.WriteLine();
            }
        }
    }
}
