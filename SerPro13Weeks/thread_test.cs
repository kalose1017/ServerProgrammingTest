using System.Threading;
using System.Net.Sockets;
using System.Data;

namespace SerPro13Weeks
{
    internal class thread_test
    {
        class Point3D
        {
            public int x, y, z;
            public Point3D(int x, int y, int z)
            {
                this.x = x; this.y = y; this.z = z;
            }
        }
        static void Main(string[] args)
        {

            Point3D pt1 = new Point3D(10, 20, 30);
            Point3D pt2 = new Point3D(40, 50, 60);
            Point3D pt3 = new Point3D(70, 80, 90);

            Thread thread1 = new Thread(MyThread);
            thread1.Start(pt1);

            Thread thread2 = new Thread(MyThread);
            thread2.Start(pt2);

            Thread thread3 = new Thread(MyThread);
            thread3.Start();

            Thread thread4 = new Thread(MyThread);
            thread1.Start();

            Thread thread5 = new Thread(MyThread);
            thread2.Start();

            Thread thread6 = new Thread(MyThread);
            thread3.Start();

            while (true)
            {
                Console.WriteLine($"Running Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        static void MyThread()
        {
            int count = Thread.CurrentThread.ManagedThreadId;
            string text = GenerateRandomString(count);
            while(true)
            {
                Console.WriteLine($"Running Thread ID: {text}");
            }
        }

        private static string GenerateRandomString(int count)
        {
            const string ch = "ABCDEFGHIJKLMNOPQRSTUVWZYZ";
        }

        static void MyThread2(object arg)
        {
            Thread.Sleep(1000);
            Point3D pt = (Point3D)arg;
            Console.WriteLine($"Running Thread data : {pt.x}, {pt.y}, {pt.z} -- ID: {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
