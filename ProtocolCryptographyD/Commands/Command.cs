namespace ProtocolCryptographyD
{
    internal abstract class Command
    {
        public static int MaxLengthData { get { return 16777199; } }

        protected TypeCom type;
        protected byte[] payLoad;
        public TypeCom typeCom { get { return type; } }
        public byte[] PayLoad { get { return payLoad; } }

        //public static Command ParseToCom(byte[] buffer)
        //{
        //    if (buffer == null || buffer.Length == 0)
        //        throw new ArgumentNullException(nameof(buffer));

        //    TypeCom typeData;
        //    byte[] payLoad = new byte[buffer.Length - 1];

        //    try
        //    {
        //        typeData = (TypeCom)buffer[0];
        //    }
        //    catch
        //    {
        //        throw new ArgumentOutOfRangeException(nameof(typeData));
        //    }
        //    Array.Copy(buffer, 1, payLoad, 0, buffer.Length - 1);

        //    switch(typeData)
        //    {
        //        case TypeCom.RSAPKEY_R:
        //            {
        //                return RsaPkeyComR.ParseToCom(payLoad);
        //            }
        //        case TypeCom.RSAPKEY_A:
        //            {
        //                return RsaPkeyComA.ParseToCom(payLoad);
        //            }
        //        case TypeCom.AESKEY_R:
        //            {
        //                return AesKeyComR.ParseToCom(payLoad);
        //            }
        //        default:
        //            {
        //                return UnknowCom.ParseToCom(payLoad);
        //            }
        //    }
        //}
    }
}
