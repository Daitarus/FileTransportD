namespace ProtocolCryptographyD
{
    public abstract class ServerRequestHandler
    {
        private Transport transport;

        public void SetTranport(Transport transport)
        {
            this.transport = transport;
        }


    }
}
