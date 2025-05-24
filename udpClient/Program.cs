using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace udpClient
{
    internal class Program
    {
        static string SERVERIP = "127.0.0.1";
        const int SERVERPORT = 9000;
        const int BUFSIZE = 1024;

        static void Main(string[] args)
        {
            int retval;

            if (args.Length > 0) SERVERIP = args[0];

            Socket sock = null;
            try
            {
                Log("Client 소켓 생성");
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            IPEndPoint serveraddr = new IPEndPoint(IPAddress.Parse(SERVERIP), SERVERPORT);
            byte[] buf = new byte[BUFSIZE];

            Console.WriteLine("UDP통신 시작");
            while (true)
            {
                Console.Write("\n[보낼 데이터] ");
                string data = Console.ReadLine();
                if (data.Equals("QUIT", StringComparison.OrdinalIgnoreCase) ||
                    data.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    Log("종료 명령어 입력됨. 소켓 종료");
                    break;
                }

                try
                {
                    byte[] senddata = Encoding.Default.GetBytes(data);
                    int size = senddata.Length;
                    if (size > BUFSIZE) size = BUFSIZE;

                    retval = sock.SendTo(senddata, 0, size, SocketFlags.None, serveraddr);
                    Log($"데이터 전송 완료 ({retval}바이트)");

                    EndPoint peeraddr = new IPEndPoint(IPAddress.Any, 0);
                    retval = sock.ReceiveFrom(buf, BUFSIZE, SocketFlags.None, ref peeraddr);

                    string received = Encoding.Default.GetString(buf, 0, retval);
                    Log($"수신 완료 ({retval}바이트) → {received}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }

            sock.Close();
        }

        static void Log(string msg)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} : {msg}");
        }
    }
}
