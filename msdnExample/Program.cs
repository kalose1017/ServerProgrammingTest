using System.Net.Sockets;
using System.Text;

namespace msdnExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SendHttpRequest(new Uri("http://www.naver.com"), 80);
        }

        private static void SendHttpRequest(Uri? uri = null, int port = 80)
        {
            uri ??= new Uri("http://example.com");

            // Construct a minimalistic HTTP/1.1 request
            byte[] requestBytes = Encoding.ASCII.GetBytes(@$"GET {uri.AbsoluteUri} HTTP/1.0
Host: {uri.Host}
Connection: Close

");

            // Create and connect a dual-stack socket
            using Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(uri.Host, port);

            // Send the request.
            // For the tiny amount of data in this example, the first call to Send() will likely deliver the buffer completely,
            // however this is not guaranteed to happen for larger real-life buffers.
            // The best practice is to iterate until all the data is sent.
            int bytesSent = 0;
            while (bytesSent < requestBytes.Length)
            {
                bytesSent += socket.Send(requestBytes, bytesSent, requestBytes.Length - bytesSent, SocketFlags.None);
            }

            // Do minimalistic buffering assuming ASCII response
            byte[] responseBytes = new byte[256];
            char[] responseChars = new char[256];

            while (true)
            {
                int bytesReceived = socket.Receive(responseBytes);

                // Receiving 0 bytes means EOF has been reached
                if (bytesReceived == 0) break;

                // Convert byteCount bytes to ASCII characters using the 'responseChars' buffer as destination
                int charCount = Encoding.ASCII.GetChars(responseBytes, 0, bytesReceived, responseChars, 0);

                // Print the contents of the 'responseChars' buffer to Console.Out
                Console.Out.Write(responseChars, 0, charCount);
            }
        }
    }
}
