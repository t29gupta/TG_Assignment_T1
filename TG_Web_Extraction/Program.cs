using System;

namespace TG_Web_Extraction
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            HTML_Extractiom extractiom = new HTML_Extractiom();
            var fileContents = extractiom.ReadHtmlFile();

            extractiom.Extractinfo(fileContents);

            Console.Read();
        }
    }
}
