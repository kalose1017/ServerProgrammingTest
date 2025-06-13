using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Echo_Client
{
    internal class Client
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var IPEndPoint = new IPEndPoint(IPAddress.Any, 9000);

            Console.WriteLine("Server Connecting...");
            socket.Connect(IPEndPoint);
            Console.WriteLine($"Server Connected from : {socket.RemoteEndPoint}");

            while(true)
            {
                string snedMessage = Console.ReadLine();
                byte[] sendBuffer = Encoding.UTF8.GetBytes(snedMessage);
                socket.Send(sendBuffer);
                Console.WriteLine("Message Sending...");

                byte[] recvBuffer = new byte[1024];
                int received = socket.Receive(recvBuffer);
                string recvMessage = Encoding.UTF8.GetString(recvBuffer, 0, received);
                Console.WriteLine($"Received Echo : {recvMessage}");
                
                socket.Close();
            }
        }
    }
}

/*Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, 9000);

            Console.WriteLine("Server Connecting...");
            socket.Connect(serverEndPoint);
            Console.WriteLine($"Server Connected from : {socket.RemoteEndPoint}");
            
            while(true)
            {
                string sendMessage = Console.ReadLine();
                byte[] sendBuffer = Encoding.UTF8.GetBytes(sendMessage);
                socket.Send(sendBuffer);
                Console.WriteLine("Message Sending...");

                byte[] recvBuffer = new byte[1024];
                int received = socket.Receive(recvBuffer);
                string recvMessage = Encoding.UTF8.GetString(recvBuffer, 0, received);
                Console.WriteLine($"Received Echo : {recvMessage}");
                
                socket.Close();
            }*/

/*Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, 9000);

            Console.WriteLine("Server Connecting ... ");
            clientSock.Connect(serverEndPoint);
            Console.WriteLine($"Server Connected from : {clientSock.LocalEndPoint}");

            string sendMessage = Console.ReadLine();
            byte[] sendBuffer = Encoding.UTF8.GetBytes(sendMessage);
            clientSock.Send(sendBuffer);
            Console.WriteLine("Message Sending...");

            byte[] recvBuffer = new byte[1024];
            int received = clientSock.Receive(recvBuffer);
            string recvMessage = Encoding.UTF8.GetString(recvBuffer, 0, received);
            Console.WriteLine($"Received Echo : {recvMessage}");

            clientSock.Close();*/