using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace tcpClient
{
    internal class Program
    {
        //tcp 클라이언트 경로 미지정 버전
        static string SERVERIP = "127.0.0.1";
        const int SERVERPORT = 19000;
        const int BUFSIZE = 1024;

        static List<string> logList = new List<string>();

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
                    Console.WriteLine("(gofile이라고 입력하면 txt파일에 통신내역을 저장합니다.)");
                    Console.Write("\n[보낼 데이터] ");
                    string data = Console.ReadLine();

                    if (data.Equals("QUIT", StringComparison.OrdinalIgnoreCase) ||
                        data.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                    {
                        Log("종료 명령어 입력됨. 소켓 종료");
                        break;
                    }

                    if (data.Equals("gofile", StringComparison.OrdinalIgnoreCase))
                    {
                        string filePath = "client.txt";
                        string directory = Path.GetDirectoryName(filePath);

                        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                        {
                            Log($"[오류] 경로가 존재하지 않습니다: {directory}");
                        }
                        else
                        {
                            using (StreamWriter sw = new StreamWriter(filePath))
                            {
                                foreach (string line in logList)
                                    sw.WriteLine(line);
                            }
                            Log("콘솔 출력 로그를 client.txt에 저장 완료");
                        }
                        continue;
                    }

                    byte[] senddata = Encoding.Default.GetBytes(data);
                    int size = senddata.Length;
                    if (size > BUFSIZE) size = BUFSIZE;

                    retval = sock.Send(senddata, 0, size, SocketFlags.None);
                    Log($"데이터 전송 완료: {retval}바이트");

                    retval = ReceiveN(sock, buf, retval, SocketFlags.None);
                    if (retval == 0) break;
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
            string logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm} : {msg}";
            Console.WriteLine(logMsg);
            logList.Add(logMsg);
        }
    }
}
