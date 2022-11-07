using NLog;
using System.Text;

namespace ConsoleWorker
{
    public static class PrintMessage
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static void PrintColorMessage(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void WriteLog(string message)
        {
            logger.Info(message);
        }
    }
}
