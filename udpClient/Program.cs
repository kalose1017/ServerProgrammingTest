using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace udpClient
{
    internal class Program
    {
        //udp 클라이언트 절대 경로 지정
        static string SERVERIP = "127.0.0.1";
        const int SERVERPORT = 9000;
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
                Console.WriteLine("(gofile이라고 입력하면 로그가 파일로 저장됩니다)");
                Console.Write("\n[보낼 데이터] ");
                string data = Console.ReadLine();
                if (data.Equals("QUIT", StringComparison.OrdinalIgnoreCase) ||
                    data.Equals("EXIT", StringComparison.OrdinalIgnoreCase))
                {
                    Log("종료 명령어 입력됨. 소켓 종료");
                    break;
                }

                // gofile 명령어 처리
                if (data.Equals("gofile", StringComparison.OrdinalIgnoreCase))
                {
                    string filePath = "C:/Users/leeji/OneDrive/바탕 화면/ServerProgrammingTest/udpClient/bin/Debug/net8.0/logs/client.txt";
                    string directory = Path.GetDirectoryName(filePath);

                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory); // logs 폴더 자동 생성
                    }

                    File.WriteAllLines(filePath, logList);
                    Log($"로그를 {filePath}에 저장 완료");
                    continue;
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
            string logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm} : {msg}";
            Console.WriteLine(logMsg);
            logList.Add(logMsg); // 로그 리스트에 저장
        }
    }
}
