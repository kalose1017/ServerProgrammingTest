using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SerPro12Weeks
{
    internal class Program
    {
        const int SERVERPORT = 19000;
        const int BUFSIZE = 1024;
        static void Main(string[] args)
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, SERVERPORT);
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Echo Server Started!");
            while (true) 
            {
                Socket clientSocket = listener.Accept();
                Console.WriteLine($"Client Connected : {clientSocket.RemoteEndPoint}");

                byte[] buf = new byte[BUFSIZE];

                int received = clientSocket.Receive(buf);
                // Message Processing
                string recvText = Encoding.UTF8.GetString(buf, 0, received);
                Console.WriteLine($"Received : {recvText}");
                clientSocket.Send(Encoding.UTF8.GetBytes(recvText));
                clientSocket.Close();
            }

        }
    }
}
