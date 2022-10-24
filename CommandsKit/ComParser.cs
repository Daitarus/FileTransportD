using ProtocolCryptographyD;

namespace CommandsKit
{
    public class ComParser : IParser
    {
        public ICommand Parse(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                throw new ArgumentNullException(nameof(buffer));

            TypeCom typeData;
            byte[] payLoad = new byte[buffer.Length - 1];

            try
            {
                typeData = (TypeCom)buffer[0];
            }
            catch
            {
                throw new ArgumentOutOfRangeException(nameof(typeData));
            }
            Array.Copy(buffer, 1, payLoad, 0, buffer.Length - 1);

            switch (typeData)
            {
                case TypeCom.AUTHORIZATION_R:
                    {
                        return AuthorizationComR.BytesToCom(payLoad);
                    }
                case TypeCom.AUTHORIZATION_A:
                    {
                        return AuthorizationComA.BytesToCom(payLoad);
                    }
                default:
                    {
                        return UnknowCom.BytesToCom(payLoad);
                    }
            }
        }
    }
}
