using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;

namespace ClassLibrary_Echo
{
    public class ChatClient
    {
        const int SERVERPORT = 19000;
        const int BUFSIZE = 1024;
        private IPEndPoint serverEndPoint;
        private Socket clientSocket;
        private Thread RecvThread;

        public ChatClient(IPEndPoint ipEndPoint)
        {
            this.serverEndPoint = ipEndPoint;
            serverEndPoint = new IPEndPoint(IPAddress.Loopback, SERVERPORT);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Close()
        {
            if(clientSocket != null && clientSocket.Connected) 
            {
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    Thread.Sleep(100);
                    clientSocket.Close();
                }
                catch { }
            }
            clientSocket = null;
            if(RecvThread != null && RecvThread.IsAlive)
            {
                try
                {
                    RecvThread.Interrupt();
                    RecvThread.Join(1000);
                }
                catch (ThreadInterruptedException ex) 
                {
                    Console.WriteLine($"Thread Error : {ex.Message}");
                }
            }
        }
       
        public bool Connect()
        {
            try
            {
                if(clientSocket != null && clientSocket.Connected)
                {
                    Console.WriteLine("Already Connected");
                    return false;
                }
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Console.WriteLine("Server Connecting...");
                clientSocket.Connect(serverEndPoint);
                Console.WriteLine("Server Connected!!");

                RecvThread = new Thread(ReceiveMessage);
                RecvThread.IsBackground = true;
                RecvThread.Start();
                //ThreadPool.QueueUserWorkItem(ReceiveMessage, null);

                return true;
            }
            catch(SocketException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public void SendMessage(string input)
        {
            if (string.IsNullOrEmpty(input)) return;
            
            if(clientSocket == null || !clientSocket.Connected) 
            {
                Console.WriteLine($"not Connected");
                return;
            }

            byte[] sendbuf = Encoding.UTF8.GetBytes(input);
            try
            {
                clientSocket.Send(sendbuf);

            }
            catch(SocketException e)
            {
                Console.WriteLine($"socket Error : {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : {e.Message}");
            }

        }
        public void ReceiveMessage() 
        {
            byte[] recvbuf = new byte[BUFSIZE];
            int received;
            while(true)
            {
                try
                {
                    received = clientSocket.Receive(recvbuf);
                    if (received == -1 || received == 0)
                    {
                        Console.WriteLine($"Disconnecting {clientSocket.RemoteEndPoint}");
                        break;
                    }
                    string msg = Encoding.UTF8.GetString(recvbuf);
                    Console.WriteLine($"Msg-Recv => {clientSocket.RemoteEndPoint}::{msg}");
                }
                catch(SocketException ex)
                {
                    Console.WriteLine($"Socket Error {ex.Message}");
                }
                catch(Exception ex) 
                {
                    Console.WriteLine($"General Exception {ex.Message}");
                }
            }
            Close();

        }

    }
}
