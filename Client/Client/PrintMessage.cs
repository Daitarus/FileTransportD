namespace Client
{
    public class PrintMessage
    {
        public static void PrintColorMessage(string message, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(message);
            Console.ResetColor();
        }
    }
}
