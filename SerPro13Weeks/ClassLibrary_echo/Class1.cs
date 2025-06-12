namespace ClassLibrary_echo
{
    public class Class1
    {
        private readonly int port;

        public EchoSimpleServer(int port, MessageProcessor processor = null)
        {
            this.port = port;
            this.processor = processor ?? default_MessageProcessor;
        }
    }
}
