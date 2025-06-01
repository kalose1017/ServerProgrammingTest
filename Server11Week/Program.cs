using System.Diagnostics.Contracts;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Server11Week
{
    internal class Program
    {
        static void AnalyzeString(string input, out int length, out int spaceCount, out int digitCount, out int upperCount, out int lowerCount)
        {
            char[] str = input.ToCharArray();
            length = str.Length;
            spaceCount = 0; digitCount = 0; upperCount = 0; lowerCount = 0; 
            for(int i = 0; i < length; i++) 
            {
                if (str[i] == ' ') spaceCount++;
                if (str[i] >= 48 && str[i] <= 57) digitCount++;
                if (str[i] >= 65 && str[i] <= 90) upperCount++;
                if (str[i] >= 97 && str[i] <= 122) lowerCount++;
            }
        }

        static void Main(string[] args)
        {
            Console.Write("문자열을 입력하세요: ");
            string input = Console.ReadLine();

            AnalyzeString(input, out int len, out int spaces, out int digits, out int uppers, out int lowers);

            Console.WriteLine($"총 길이: {len}");
            Console.WriteLine($"공백 수: {spaces}");
            Console.WriteLine($"숫자 수: {digits}");
            Console.WriteLine($"대문자 수: {uppers}");
            Console.WriteLine($"소문자 수: {lowers}");
        }

    }
}
