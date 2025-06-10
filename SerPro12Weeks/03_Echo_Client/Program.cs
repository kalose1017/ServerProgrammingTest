using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace _03_Echo_Client
{
    internal class Program
    {
        const int SERVERPORT = 19000;
        const int BUFSIZE = 1024;
        static void Main(string[] args)
        {
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, SERVERPORT);
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Server Connecting...");
            clientSocket.Connect(serverEndPoint);
            Console.WriteLine("Server Connected!!");

            string data = Console.ReadLine();
            byte[] sendbuf = Encoding.UTF8.GetBytes(data);
            clientSocket.Send(sendbuf);

            byte[] recvbuf = new byte[BUFSIZE];
            int received = clientSocket.Receive(recvbuf);
            string redata = Encoding.UTF8.GetString(recvbuf, 0, received);

            Console.WriteLine($"Echo from Server : {redata}");
            clientSocket.Close();
        }
    }
}
