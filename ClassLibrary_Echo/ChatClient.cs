using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_Echo
{
    public class ChatClient
    {
        private IPEndPoint serverEndPoint;
        private Socket clientSock;
        private Thread RecvThread;

        public ChatClient(IPEndPoint iPEndPoint)
        {
            this.serverEndPoint = iPEndPoint;
            //clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Close()
        {
            if (clientSock != null && clientSock.Connected)
            {
                try
                {
                    clientSock.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(100); // waiting
                    clientSock.Close();
                }
                catch { }
            }
            clientSock = null;

            // recvThread interrupt
            if (RecvThread != null && RecvThread.IsAlive)
            {
                try
                {
                    RecvThread.Interrupt();
                    Thread.Sleep(100);
                    RecvThread.Join(100);
                }
                catch (Exception ex) { }
            }

            Console.WriteLine($"[info-lib] connection is closed");
        }
        public bool Connect()
        {
            try
            {
                // re-try connection 
                if (clientSock != null && clientSock.Connected)
                {
                    Console.WriteLine("[info-lib] Already connected");
                    return false;
                }

                clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Server Connecting (library) ... ");
                clientSock.Connect(serverEndPoint);
                Console.WriteLine($"Server Connected from : {clientSock.LocalEndPoint}");

                RecvThread = new Thread(RecvMessageProcessor);
                RecvThread.IsBackground = true;
                RecvThread.Start();
                //ThreadPool.QueueUserWorkItem(RecvMessageProcessor, null); // 비교용

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Exception : {ex.Message}");
                return false;
            }
        }
        public void SendMessage(string input)
        {
            if (string.IsNullOrEmpty(input)) return;

            if (clientSock == null || !clientSock.Connected)
            {
                Console.WriteLine($"[info-warn] Server not connected");
                return;
            }

            byte[] data = Encoding.UTF8.GetBytes(input);
            try
            {
                clientSock.Send(data);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"[lib-err] : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[lib-err] : {ex}");
            }
        }
        private void RecvMessageProcessor(object? obj)
        {
            byte[] recvBuffer = new byte[1400];
            int received;

            while (true)
            {
                try
                {
                    received = clientSock.Receive(recvBuffer);
                    // 3초 정도 대기하다가 데이터가 없으면 종료하겠다.

                    if (received == -1 || received == 0)
                    {
                        Console.WriteLine($"[info-lib] Disconnecting from {clientSock.RemoteEndPoint}");
                        break;
                    }

                    string msg = Encoding.UTF8.GetString(recvBuffer);
                    Console.WriteLine($"\n{msg}");

                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"[lib-err] : {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[lib-err] : {ex}");
                }
            }
            clientSock.Close();
        }
    }
}
