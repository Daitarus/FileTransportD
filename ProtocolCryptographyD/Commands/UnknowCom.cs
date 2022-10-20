namespace ProtocolCryptographyD
{
    internal class UnknowCom : Command
    {
        public UnknowCom()
        {
            type = TypeCom.UNKNOW;
            payLoad = new byte[0];
        }
        public UnknowCom(byte[] payLoad)
        {
            type = TypeCom.UNKNOW;
            if (payLoad == null)
            {
                payLoad = new byte[0];
            }
            else
            {
                this.payLoad = payLoad;
            }
        }

        public static byte[] ConvertToBytes(UnknowCom com)
        {
            byte[] bytes = new byte[1];

            if (com.PayLoad != null)
            {
                bytes = new byte[com.PayLoad.Length + 1];
                bytes[0] = (byte)com.typeCom;
                Array.Copy(com.PayLoad, 0, bytes, 1, com.PayLoad.Length);
            }
            bytes[0] = (byte)com.typeCom;

            return bytes;
        }

        public static UnknowCom ParseToCom(byte[] payLoad)
        {
            return new UnknowCom(payLoad);
        }
    }
}
