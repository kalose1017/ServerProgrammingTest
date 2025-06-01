using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Server11Week
{
    static class Constants
    {
        public static string TEXTFILE = "DebugLog.txt";
        public static string JSONFILE = "DebugLog.json";

    }
    public interface FileIO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public void Initialize();
        public void Read();
        public void Write(string text);
        public void Append(string text);
    }
    class TextFile : FileIO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public TextFile(string fileName)
        {
            Console.WriteLine("객체 생성");
            this.FileName = fileName;
            string basePaht = Directory.GetCurrentDirectory();
            string relativePath = Path.Combine("data");
            FilePath = Path.Combine(basePaht, relativePath);
            Console.WriteLine($"상대 경로: {relativePath}");
            Console.WriteLine($"결합된 전체 경로: {FilePath}");
            if (!string.IsNullOrEmpty(FilePath) && !Directory.Exists(FilePath))
            {
                try
                {
                    Directory.CreateDirectory(FilePath);
                    Console.WriteLine("디렉터리 생성");

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"디렉터리 생성 오류 : {ex.Message}");
                    throw;
                }
            }
            FilePath = Path.Combine(FilePath, FileName);
            Console.WriteLine($"결합된 전체 경로: {FilePath}");
        }
        public void Initialize()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                    Console.WriteLine("파일 생성");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw;
            }
        }
        public void Write(string text)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8))
                {
                    sw.WriteLine(text);
                    Console.WriteLine($"파일에 내용 쓰기 완료(덮어쓰기) : {text}");
                    Console.WriteLine($"파일에 내용 쓰기 완료(덮어쓰기) : {FilePath}");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void Append(string text)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(text);
                    Console.WriteLine($"파일에 내용 쓰기 완료 : {text}");
                    Console.WriteLine($"파일에 내용 쓰기 완료(덮어쓰기) : {FilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일 쓰기 오류 : {ex.Message}");
            }
        }
        public void Read()
        {

            try
            {
                //if (!File.Exists(FilePath))
                //{
                //    Console.WriteLine($"읽을 파일이 존재하지 않습니다.{FilePath}");

                //}
                using (StreamReader sr = new StreamReader(FilePath, Encoding.UTF8))
                {
                    string Content = sr.ReadToEnd();
                    Console.WriteLine($"파일 내용 읽기 완료 : {FilePath}");
                    Console.WriteLine($"{Content}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일 읽기 오류 : {ex.Message}");
                throw;
            }
        }


    }
    class JsonFile : FileIO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        //json 구조 더 짜기
        public JsonFile(string fileName)
        {
            Console.WriteLine("객체 생성");
            this.FileName = fileName;
            string basePaht = Directory.GetCurrentDirectory();
            string relativePath = Path.Combine("data", fileName);
            FilePath = Path.Combine(basePaht, relativePath);
            Console.WriteLine($"상대 경로: {relativePath}");
            Console.WriteLine($"결합된 전체 경로: {FilePath}");
            if (!string.IsNullOrEmpty(FilePath) && !Directory.Exists(FilePath))
            {
                try
                {
                    Directory.CreateDirectory(FilePath);
                    Console.WriteLine("디렉터리 생성");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"디렉터리 생성 오류 : {ex.Message}");
                    throw;
                }
            }

        }
        public void Initialize()
        {

        }
        public void Append(string text)
        {

        }
        public void Read()
        {
            string text = " ";
            try
            {
                if (!File.Exists(FilePath))
                {
                    Console.WriteLine($"읽을 파일이 존재하지 않습니다.{FilePath}");

                }
                using (StreamReader sr = new StreamReader(FilePath, Encoding.UTF8))
                {
                    string Content = sr.ReadToEnd();
                    Console.WriteLine($"파일 내용 읽기 완료 : {FilePath}");
                    Console.WriteLine($"{Content}");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void Write(string text)
        {

        }
    }
    class Log
    {
        public string TimeStemp { get; set; }
        public string DebugLog { get; set; }


    }
    internal class Program
    {
        static void Main(string[] args)
        {
            //constans
            Console.WriteLine("Hello, World!");
            string time = DateTime.Now.ToString();
            TextFile textFile = new TextFile(Constants.TEXTFILE);
            textFile.Initialize();
            textFile.Read();
            textFile.Append("asdasd");

        }
    }
}
