using ProtocolCryptographyD;

namespace CommandsKit
{
    internal static class ExecuteAnswer
    {
        public static bool Authentication(Transport transport, ref ClientInfo clientInfo, bool answer)
        {
            clientInfo.authentication = answer;
            return answer;
        }

        public static bool Registration(Transport transport, ref ClientInfo clientInfo, bool answer)
        {
            return answer;
        }
    }
}
