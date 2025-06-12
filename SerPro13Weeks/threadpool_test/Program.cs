namespace threadpool_test
{
    internal class Program
    {
        static int sum = 0;
        static void MyThread(object arg)
        {
            int num = (int)arg;
            for(int i = 1; i <= num; i++)
            {
                sum += i;
            }
        }
        static void Calc(object rad)
        {
            if (rad == null) return;
            double r = (double)rad;
            double area = r * r * 3.141592;

            Console.WriteLine($"r={r}, area={area}");
        }
        static void Main(string[] args)
        {
            int num = 100;
            Thread thread1 = new Thread(MyThread);
            Console.WriteLine($"Before Thread Run : {sum}");
            thread1.Start(num);
            thread1.Join();
            Console.WriteLine($" after Thread run : {sum}");

            for(double i = 0; i < 65536; i++)
            {
                ThreadPool.QueueUserWorkItem(Calc, i);
            }
            Console.ReadLine();
        }
    }
}
