using ClassLibrary_Echo;

namespace Echo_Server_Class
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Echo Simple Server Start");
            var myServer = new ClassLibrary_Echo.EchoSimpleServer(19000, myProcessor);
            myServer.Start();
        }

        private static string myProcessor(string input)
        {
            return $"[external MSG] {input} !!!!";
        }
    }

}
