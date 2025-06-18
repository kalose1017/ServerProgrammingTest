using ClassLibrary_Echo;

namespace chatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Chat Server !");
            // 서버 객체 생성
            var server = new ChatMultiUserServer(9000);
            // 서버 실행
            // UI or Game Engine 이 실행되는 도중에 서버코드에서
            // 멈춰있으면 안된다.
            server.Start(); // <<----- thread 넘겨서 실행 필요
            // 구버전 제어 권한이 넘어오지 않는다
            // 수정버전 제어권이 넘어왓지만 Thread 가 종료될때 까지 대기
            Console.WriteLine("[info-app] i alive in mainThread");
        }
    }
}
