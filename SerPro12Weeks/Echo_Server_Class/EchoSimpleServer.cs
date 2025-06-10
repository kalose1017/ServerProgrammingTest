using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Echo_Server_Class
{
    class EchoSimpleServer
    {
        public delegate string MessageProcessor(string message);

        private readonly MessageProcessor processor;
        private readonly int port;

        public EchoSimpleServer(int port, MessageProcessor processor = null)
        {
            this.port = port;
            this.processor = processor ?? default_MessageProcessor;
        }
        public void Start()
        {
            const int BUFSIZE = 1024;

            var localEndPoint = new IPEndPoint(IPAddress.Any, port);
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(10);

            Console.WriteLine("Echo Server Started!");
            while (true)
            {
                Socket clientSocket = listener.Accept();
                Console.WriteLine($"Client Connected : {clientSocket.RemoteEndPoint}");
                string clientEP = clientSocket.RemoteEndPoint.ToString();
                byte[] buf = new byte[BUFSIZE];

                int received = clientSocket.Receive(buf);
                #region Message Processing
                    string recvText = Encoding.UTF8.GetString(buf, 0, received);
                    Console.WriteLine($"Received : {recvText}");

                    // Message Processing Core in Server
                    string response = processor(recvText);
                    
                    // Message Echo re-send
                    clientSocket.Send(Encoding.UTF8.GetBytes(recvText));
                    clientSocket.Close();
                    Console.WriteLine($"Client disconnected : {clientEP}");
                #endregion
            }
        }

        private string default_MessageProcessor(string recvText)
        {
            //return $"[re-send MSG] : {recvText}";
            return $"[default MSG] : {recvText}";

        }
    }
}
