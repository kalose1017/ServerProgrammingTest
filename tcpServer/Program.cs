using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcpServer
{
    internal class Program
    {
        const int SERVERPORT = 19000;
        const int BUFSIZE = 2048;

        static void Main(string[] args)
        {
            int retval;
            Socket listen_sock = null;

            try
            {
                Log("서버 소켓 생성");
                listen_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listen_sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 100);
                listen_sock.Bind(new IPEndPoint(IPAddress.Any, SERVERPORT));
                listen_sock.Listen(2);
                Log("서버 대기 시작");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            byte[] buf = new byte[BUFSIZE];

            while (true)
            {
                Socket client_sock = null;
                IPEndPoint clientaddr = null;

                try
                {
                    client_sock = listen_sock.Accept();
                    clientaddr = (IPEndPoint)client_sock.RemoteEndPoint;
                    Log($"클라이언트 접속: IP 주소={clientaddr.Address}, 포트 번호={clientaddr.Port}");

                    // Welcome 메시지 전송
                    string welcome = "하와와~ 이지호의 TCP서버에 오신 것을 환영티비!";
                    client_sock.Send(Encoding.Default.GetBytes(welcome));
                    Log("Welcome 메시지 전송 완료");

                    while (true)
                    {
                        retval = client_sock.Receive(buf, BUFSIZE, SocketFlags.None);
                        if (retval == 0) break;

                        string received = Encoding.Default.GetString(buf, 0, retval);
                        Log($"데이터 수신 완료: {received}");

                        client_sock.Send(buf, retval, SocketFlags.None);
                        Log("데이터 전송 완료");
                    }

                    client_sock.Close();
                    Log($"클라이언트 종료: IP 주소={clientaddr.Address}, 포트 번호={clientaddr.Port}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }

            listen_sock.Close();
        }

        static void Log(string msg)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} : {msg}");
        }
    }
}
