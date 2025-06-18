using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_Echo
{
    public class ChatMultiUserServer
    {
        private readonly int port;
        private List<Socket> clientSocketList;
        private bool isRunningServer;
        private Socket serverSock;
        private Thread listenerThread;
        private object syncLock;

        public ChatMultiUserServer(int port)
        {
            this.port = port;
            isRunningServer = false;
            syncLock = new();
            clientSocketList = new List<Socket>();
        }
        public void Stop()
        {
            if (!isRunningServer)
            {
                Console.WriteLine("[info-lib] Server is not running.");
                return;
            }

            listenerThread.Interrupt();  // userHandler Thread 들은 background thread로 listener Thread 종료와 함께 종료됨
            Thread.Sleep(100);          // Thread 종료를 수동적으로 기다림 
            listenerThread.Join(100);  // 지정된 Thread의 종료 요청 후 이곳에서 대기
            isRunningServer = false;

            serverSock.Shutdown(SocketShutdown.Both);
            Thread.Sleep(100);
            serverSock.Close();
        }
        public void Start()
        {
            if (isRunningServer)
            {
                Console.WriteLine("[info-lib] Server is alerady running.");
                return;
            }

            serverSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var localServerEndPoint = new IPEndPoint(IPAddress.Any, port);
            serverSock.Bind(localServerEndPoint);
            serverSock.Listen();
            Console.WriteLine("Echo Server Started : (MultiUser libarary Class)");

            isRunningServer = true;

            listenerThread = new Thread(StartWithThreadPool);
            listenerThread.IsBackground = false;
            listenerThread.Start(null);
            //ThreadPool.QueueUserWorkItem(StartWithThreadPool, null); // background thread 비교용
        }
        private void StartWithThreadPool(object arg)
        {
            while (true)
            {
                Socket clientSock = serverSock.Accept();
                lock (syncLock)
                {
                    clientSocketList.Add(clientSock);
                }
                ThreadPool.QueueUserWorkItem(userHandler, clientSock);
            }
        }
        private void userHandler(object arg)
        {
            Socket clientSock = (Socket)arg;
            Console.WriteLine($"Connecte : {clientSock.RemoteEndPoint}");
            string clientEP = clientSock.RemoteEndPoint.ToString();
            try
            {
                while (true)
                {
                    // Data receive
                    byte[] buffer = new byte[1024];
                    int received = clientSock.Receive(buffer);
                    if (received == -1 || received == 0)
                    {
                        Console.WriteLine($"[info-lib] Disconnecting {clientSock.RemoteEndPoint}");
                        break;
                    }
                    // Message Recv
                    string msg = Encoding.UTF8.GetString(buffer);
                    // 서버에서클라이언트의 메세지를 볼 필요가 있나??
                    // 로그로 메세지 내용을 기록할 필요가 있나??
                    Console.WriteLine($"[SVR-RECV] : {clientSock.RemoteEndPoint} << {msg.Length} bytes");

                    // Chat BroadCasting
                    messageBroadcast(msg, clientSock);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"[lib-err] Socket Error : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[lib-err] General Error : {ex.Message}");
            }
            finally
            {
                lock (syncLock)
                {
                    clientSocketList.Remove(clientSock);
                }
                //Console.WriteLine($"Client Disconnected : {clientSock.RemoteEndPoint}");
                Console.WriteLine($"[info-lib] Client Disconnected in exception: {clientEP}");
                clientSock.Close();
            }

        }
        private void messageBroadcast(string msg, Socket sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            lock (syncLock)
            {
                // sender를 제외한 나머지 사용자에게 전송
                foreach (Socket sock in clientSocketList)
                {
                    if (sock != sender)
                    {
                        try
                        {
                            sock.Send(data);
                            Console.WriteLine($"[info-lib] Sending completed {sock.RemoteEndPoint}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[info-lib] Sending Failed to {sock.RemoteEndPoint}");
                            Console.WriteLine($"{ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[info-lib] Skipping sender {sock.RemoteEndPoint}");
                    }
                }
            }
        }
    }
}
