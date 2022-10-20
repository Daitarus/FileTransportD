using System.Net.Sockets;

namespace ProtocolCryptographyD
{
    public class Transport
    {
        private int maxLengthPack = Command.MaxLengthData + 1;
        private const int lengthArrayLengthPayload = 3;

        private Socket socket;

        public Transport(Socket socket)
        {
            this.socket = socket;
        }

        public void SendData(byte[] payLoad)
        {
            payLoad = AddLength(payLoad);
            socket.Send(payLoad);
        }
        public byte[] GetData()
        {
            byte[] lengthPayLoadBuffer = new byte[lengthArrayLengthPayload];
            socket.Receive(lengthPayLoadBuffer);
            int lengthPayLoad = GetLength(lengthPayLoadBuffer);

            byte[] buffer = new byte[256];
            List<byte> payLoad = new List<byte>();
            int byteCounter = 0;

            for (int i = 0; i < lengthPayLoad / buffer.Length; i++)
            {
                byteCounter += socket.Receive(buffer);
                payLoad.AddRange(buffer);
            }

            buffer = new byte[lengthPayLoad - byteCounter];
            socket.Receive(buffer);
            payLoad.AddRange(buffer);


            return payLoad.ToArray();
        }

        private byte[] AddLength(byte[] payLoad)
        {
            if(payLoad == null)
                throw new ArgumentNullException(nameof(payLoad));

            if(payLoad.Length > maxLengthPack)
                throw new ArgumentException($"{nameof(payLoad)} size greater than {maxLengthPack}", nameof(payLoad));

            byte[] lengthPayLoad = new byte[lengthArrayLengthPayload];
            byte[] lengthPayLoadBuffer = BitConverter.GetBytes(payLoad.Length);
            Array.Copy(lengthPayLoadBuffer, lengthPayLoad, lengthArrayLengthPayload);

            byte[] newPayLoad = new byte[lengthPayLoad.Length + payLoad.Length];
            Array.Copy(lengthPayLoad, newPayLoad, lengthPayLoad.Length);
            Array.Copy(payLoad, 0, newPayLoad, lengthPayLoad.Length, payLoad.Length);

            return newPayLoad;
        }
        private int GetLength(byte[] lengthPayLoad)
        {
            if (lengthPayLoad == null)
                throw new ArgumentNullException(nameof(lengthPayLoad));

            byte[] lengthPayLoadBuffer = new byte[lengthPayLoad.Length + 1];
            Array.Copy(lengthPayLoad, lengthPayLoadBuffer, lengthPayLoad.Length);

            return BitConverter.ToInt32(lengthPayLoadBuffer);
        }
    }
}