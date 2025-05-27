using System.Net;

namespace Naming_resolution
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string TEST_NAME = "www.google.com";
            const string TEST_NAME2 = "www.yuhan.ac.kr";

            Console.WriteLine(@$"Domain Name : {TEST_NAME}");
            
            IPAddress addr = null;
            GetIPAddress(TEST_NAME, ref addr);

            Console.WriteLine($@"Resolved Domain -> IPaddress : {addr}");
        }

        private static void GetIPAddress(string name, ref IPAddress addr)
        {
            IPHostEntry hostent = null;

            hostent = Dns.GetHostEntry(name);
            addr = hostent.AddressList[0];
            return;
        }
    }
}
