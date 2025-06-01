using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace udpServer
{
    internal class Program
    {
        //udp 서버 절대 경로 지정
        const int SERVERPORT = 9000;
        const int BUFSIZE = 512;

        static List<string> logList = new List<string>();
        static object logLock = new object();

        static void Main(string[] args)
        {
            int retval;
            Socket sock = null;

            new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("(gofile이라고 입력하면 로그를 파일로 저장합니다)");
                    string input = Console.ReadLine();
                    if (input.Equals("gofile", StringComparison.OrdinalIgnoreCase))
                    {
                        string filePath = "C:/Users/leeji/OneDrive/바탕 화면/ServerProgrammingTest/udpServer/bin/Debug/net8.0/logs/server.txt";
                        string dir = Path.GetDirectoryName(filePath);
                        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        lock (logLock)
                        {
                            File.WriteAllLines(filePath, logList);
                        }

                        Console.WriteLine("server.txt로 저장 완료");
                    }
                }
            })
            {
                IsBackground = true
            }.Start();

            try
            {
                Log("서버 소켓 생성");
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sock.Bind(new IPEndPoint(IPAddress.Any, SERVERPORT));
                Log($"서버 바인딩 완료 (포트 {SERVERPORT})");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            byte[] buf = new byte[BUFSIZE];
            HashSet<string> knownClients = new HashSet<string>();

            while (true)
            {
                try
                {
                    IPEndPoint anyaddr = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint peeraddr = (EndPoint)anyaddr;

                    retval = sock.ReceiveFrom(buf, BUFSIZE, SocketFlags.None, ref peeraddr);
                    string message = Encoding.Default.GetString(buf, 0, retval);
                    string clientKey = peeraddr.ToString();

                    Log($"수신됨: [{clientKey}] → {message}");

                    if (!knownClients.Contains(clientKey))
                    {
                        knownClients.Add(clientKey);
                        string welcome = "하와와~ 이지호의 UDP서버에 오신 것을 환영티비!";
                        byte[] welcomeBytes = Encoding.Default.GetBytes(welcome);
                        sock.SendTo(welcomeBytes, welcomeBytes.Length, SocketFlags.None, peeraddr);
                        Log($"Welcome 메시지 전송 완료 → {clientKey}");
                    }

                    sock.SendTo(buf, 0, retval, SocketFlags.None, peeraddr);
                    Log($"에코 전송 완료 → {clientKey}");
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
            lock (logLock)
            {
                logList.Add(logMsg);
            }
        }
    }
}
