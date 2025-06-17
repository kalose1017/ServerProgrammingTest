using ClassLibrary_Echo;
namespace _05_ChatServer
{
    internal class chatServer
    {
        static void Main(string[] args)
        {
            var chatServer = new ChatMultiServer(9000);
            chatServer.Start();
        }
    }
}
