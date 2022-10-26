using ProtocolCryptographyD;

namespace CommandsKit
{
    public class ComParser : IParser
    {
        public Command Parse(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                throw new ArgumentNullException(nameof(buffer));

            TypeCommand typeData;
            byte[] payLoad = new byte[buffer.Length - 1];

            try
            {
                typeData = (TypeCommand)buffer[0];
            }
            catch
            {
                throw new ArgumentOutOfRangeException(nameof(typeData));
            }
            Array.Copy(buffer, 1, payLoad, 0, buffer.Length - 1);

            switch (typeData)
            {
                case TypeCommand.AUTHORIZATION_R:
                    {
                        return AuthenticationComR.BytesToCom(payLoad);
                    }
                case TypeCommand.AUTHORIZATION_A:
                    {
                        return AuthenticationComA.BytesToCom(payLoad);
                    }
                default:
                    {
                        return UnknowCom.BytesToCom(payLoad);
                    }
            }
        }
    }
}
