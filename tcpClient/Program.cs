using System;
using System.Net.Sockets;
using System.Text;

namespace tcpClient
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
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Log($"서버 ( {SERVERIP} ) 접속 시도");
                sock.Connect(SERVERIP, SERVERPORT);
                Log("서버 접속 완료");

                byte[] buf = new byte[BUFSIZE];
                retval = sock.Receive(buf);
                string welcome = Encoding.Default.GetString(buf, 0, retval);
                Log($"서버로부터 받은 메시지: {welcome}");

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

                    byte[] senddata = Encoding.Default.GetBytes(data);
                    int size = senddata.Length;
                    if (size > BUFSIZE) size = BUFSIZE;

                    retval = sock.Send(senddata, 0, size, SocketFlags.None);
                    Log($"데이터 입력 완료");
                    Log($"데이터 전송 완료: {retval}바이트");

                    retval = ReceiveN(sock, buf, retval, SocketFlags.None);
                    if (retval == 0) break;

                    //Log($"데이터 수신 완료: {retval}바이트");
                    //Console.WriteLine("[받은 데이터] " + Encoding.Default.GetString(buf, 0, retval));
                }

                sock.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }

        static int ReceiveN(Socket sock, byte[] buf, int len, SocketFlags flags)
        {
            int received, offset = 0, left = len;

            while (left > 0)
            {
                received = sock.Receive(buf, offset, left, flags);
                if (received == 0) break;
                left -= received;
                offset += received;
            }
            return len - left;
        }

        static void Log(string msg)
        {
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm} : {msg}");
        }
    }
}
