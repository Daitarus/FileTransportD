using ProtocolTransport;

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

            if (Enum.IsDefined(typeof(TypeCommand), buffer[0]))
            {
                typeData = (TypeCommand)buffer[0];
            }
            else
            {
                typeData = TypeCommand.UNKNOW;
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
                case TypeCommand.LS_R:
                    {
                        return LsComR.BytesToCom(payload);
                    }
                case TypeCommand.LS_A:
                    {
                        return LsComA.BytesToCom(payload);
                    }
                case TypeCommand.FILE_GET_R:
                    {
                        return FileGetComR.BytesToCom(payload);
                    }
                case TypeCommand.FILE_GET_A:
                    {
                        return FileGetComA.BytesToCom(payload);
                    }
                case TypeCommand.FILE_ADD_R:
                    {
                        return FileAddComR.BytesToCom(payload);
                    }
                case TypeCommand.FILE_ADD_A:
                    {
                        return FileAddComA.BytesToCom(payload);
                    }
                default:
                    {
                        return UnknowCom.BytesToCom(payload);
                    }
            }
        }      
    }
}
