using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace Echo_Server
{
    internal class Server
    {
        static void Main(string[] args)
        {
            Socket serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localEndPoint = new IPEndPoint(IPAddress.Any, 9000);
            serverSock.Bind(localEndPoint);
            serverSock.Listen();
            Console.WriteLine("Echo Server Started");

            Socket clientSock = null;
            string clientEP = null;
            while(true)
            {
                clientSock = serverSock.Accept();
                Console.WriteLine($"Connect : {clientSock.RemoteEndPoint}");
                clientEP = clientSock.RemoteEndPoint.ToString();

                byte[] buffer = new byte[1024];
                int received = clientSock.Receive(buffer);

                string recvText = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"Received : {recvText}");

                byte[] sendBuffer = Encoding.UTF8.GetBytes(recvText);
                clientSock.Send(sendBuffer);

                if (received == 0) break;
            }
            clientSock.Close();
            Console.WriteLine($"Client Disconnected : {clientEP}");
        }
    }
}

/*Socket serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            var localServerEndPoint = new IPEndPoint(IPAddress.Any, 9000);
            serverSock.Bind(localServerEndPoint);
            serverSock.Listen();
            Console.WriteLine("Echo Server Started");

            while (true)
            {
                Socket clientSock = serverSock.Accept();
                Console.WriteLine($"Connect : {clientSock.RemoteEndPoint}");
                string clientEP = clientSock.RemoteEndPoint.ToString();

                byte[] buffer = new byte[1024];
                int received = clientSock.Receive(buffer);

                string recvText = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"Received : {recvText}");

                byte[] sendBuffer = Encoding.UTF8.GetBytes(recvText);
                clientSock.Send(sendBuffer);

                clientSock.Close();
                Console.WriteLine($"Client Disconnected : {clientEP}");
            }*/