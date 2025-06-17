using ClassLibrary_Echo;
using System.Net;

namespace _05_ChatClient
{
    internal class chatClient
    {
        static void Main(string[] args)
        {
            var serverEndPoint = new IPEndPoint(IPAddress.Loopback, 9000);
            var chatClient = new ChatClient(serverEndPoint);

            if(chatClient.Connect())
            {
                Console.WriteLine("Server Connected!!");
            }
            else
            {
                Console.WriteLine("Server Connected!!");
            }
            // nickname 설정구간
            // nickname string 붙여서
            Console.WriteLine("Enter your nickname : {default:AA}");
            string nickname = Console.ReadLine();
            if(string.IsNullOrEmpty(nickname))
            {
                nickname = "AA";
            }

            while(true)
            {
                Console.WriteLine("Enter Message : ");
                string input = Console.ReadLine();

                chatClient.SendMessage($"{nickname} : {input}");
            }
            chatClient.Close();

        }
    }
}
