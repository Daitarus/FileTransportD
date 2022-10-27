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
            byte[] payload = new byte[buffer.Length - 1];

            try
            {
                typeData = (TypeCommand)buffer[0];
            }
            catch
            {
                throw new ArgumentOutOfRangeException(nameof(typeData));
            }
            Array.Copy(buffer, 1, payload, 0, buffer.Length - 1);

            switch (typeData)
            {
                case TypeCommand.AUTHORIZATION_R:
                    {
                        return AuthenticationComR.BytesToCom(payload);
                    }
                case TypeCommand.AUTHORIZATION_A:
                    {
                        return AuthenticationComA.BytesToCom(payload);
                    }
                case TypeCommand.REGISTRATION_R:
                    {
                        return RegistrationComR.BytesToCom(payload);
                    }
                case TypeCommand.REGISTRATION_A:
                    {
                        return RegistrationComA.BytesToCom(payload);
                    }
                default:
                    {
                        return UnknowCom.BytesToCom(payload);
                    }
            }
        }
    }
}
