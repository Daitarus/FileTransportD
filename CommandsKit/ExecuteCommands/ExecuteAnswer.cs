using ProtocolCryptographyD;

namespace CommandsKit
{
    internal static class ExecuteAnswer
    {
        public static void Authentication(Transport transport, ref ClientInfo clientInfo, bool answer)
        {
            clientInfo.authentication = answer;
        }
    }
}
