using System;

namespace SerPro10Weeks
{
    //202127064 이지호
    class Dog
    {
        public string dogName = "포메라니안";
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int Num = 30123;
            if (Num is int) Console.WriteLine("Num은 정수형입니다!");
            else Console.WriteLine("Num은 문자형입니다!");

            double? dnum = Num as double?;
            if (dnum != null) Console.WriteLine("실수형으로 변환 성공");
            else Console.WriteLine("변환에 실패하여 Null 반환");

            Dog dog = new Dog();
            dog = null;
            Console.WriteLine(dog?.dogName);

            string str = null;
            //str = "하하";
            string res = str ?? "str은 널!";
            Console.WriteLine(res);

            int? a = null;
            a ??= 123;
            Console.WriteLine(a);

            Func<float, float> rect = x => x * x;
            Console.WriteLine(rect(3));

            Console.WriteLine(nameof(Dog));

            global::System.Console.WriteLine("이거슨 전역 네임스페이스 지정자이와요~");
        }
    }
}
