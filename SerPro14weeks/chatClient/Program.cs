using System.Net;
using ClassLibrary_Echo;

namespace chatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Chat Client!");
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 9000);

            var client = new ChatClient(serverEP);
            if (client.Connect() == true)
            {
                Console.Write("닉네임을 입력하세요.(default :: AAA) : ");
                string nickname = Console.ReadLine();
                if (string.IsNullOrEmpty(nickname))
                {
                    nickname = "AAA";
                }

                while (true)
                {
                    Console.Write("문자를 입력하세요 : ");
                    string userText = Console.ReadLine();

                    client.SendMessage($"[{nickname}] {userText}");
                }
            }

            client.Close();
            return;
        }
    }
}
