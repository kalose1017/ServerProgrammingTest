using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace ClassLibrary_Echo
{
    public class ChatMultiServer
    {
        private readonly int port;
        private List<Socket> socketList;
        private bool isRunningServer;
        private Thread serverMainThread;
        private Socket listener;
        private object syncLock;

        public ChatMultiServer(int port)
        {
            this.port = port;
            socketList = new();
            isRunningServer = false;
            serverMainThread = new Thread(StartWithTheardPool);
            syncLock = new();
        }

        public void Start()
        {
            if (isRunningServer)
            {

            }
            var localEndPoint = new IPEndPoint(IPAddress.Any, port);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            listener.Bind(localEndPoint);
            listener.Listen(10);

            isRunningServer = true;

            serverMainThread.IsBackground = false;
            serverMainThread.Start();
        }
        public void Stop()
        {
            if (!isRunningServer)
            {

            }
            serverMainThread.Interrupt();
            Thread.Sleep(100);
            isRunningServer = false;
            listener.Close();
        }
        public void StartWithTheardPool()
        {
            while (true)
            {
                Socket clientSocket = listener.Accept();
                //브로드캐스팅을 위한 리스트 업데이트
                lock (syncLock)
                {
                    socketList.Add(clientSocket);
                }
                Console.WriteLine($"New Client : {clientSocket.RemoteEndPoint}");
                ThreadPool.QueueUserWorkItem(UserHandler, clientSocket);
            }
        }

        private void UserHandler(object arg)
        {
            const int BUFSIZE = 1024;
            Socket clientSocket = (Socket)arg;
            Console.WriteLine($"Client Connected : {clientSocket.RemoteEndPoint}");
            string clientEP = clientSocket.RemoteEndPoint.ToString();
            byte[] buf = new byte[BUFSIZE];
            Console.WriteLine("Echo Server Started!");
            try
            {
                while (true)
                {
                    int received = clientSocket.Receive(buf);
                    if (received == -1 || received == 0)
                    {
                        Console.WriteLine($"Disconnecting {clientSocket.RemoteEndPoint}");
                        clientSocket.Close();
                        break;
                    }
                    #region Message Processing
                    string recvText = Encoding.UTF8.GetString(buf, 0, received);
                    Console.WriteLine($"Received : {received}-{clientSocket.RemoteEndPoint}");

                    messageBroadcasting(recvText, clientSocket);

                    //if(!response)
                    //{
                    //    Console.WriteLine("");
                    //}

                    // Message Echo re-send
                    clientSocket.Send(Encoding.UTF8.GetBytes(recvText));
                    #endregion
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message); 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                lock(syncLock)
                {
                   socketList.Remove(clientSocket);
                }
                clientSocket.Close();
            }
        }

        private void messageBroadcasting(string recvText, Socket sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(recvText);

            lock (syncLock)
            {
                foreach (Socket sock in socketList)
                {
                    if (sock != sender)
                    {
                        try
                        {
                            sock.Send(data);
                            Console.WriteLine($"Send Success!! : {sock.RemoteEndPoint}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Send failed to : {sock.RemoteEndPoint}");
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        // 본인한테 보내지 않기
                        Console.WriteLine($"Skipping Sender {sock.RemoteEndPoint}");
                    }
                }
            }


        }
    }
}
