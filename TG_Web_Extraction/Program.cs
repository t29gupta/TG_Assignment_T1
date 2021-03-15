using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TG_Web_Extraction
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HQ Application for Task 1 : Web Extraction...\n\n");

            HTML_Extractiom extraction = new HTML_Extractiom();

            string filePath = @"E:\Cloud Drives\OneDrive\Documents\HQ plus - Backend Development Assignment\HQ plus - Backend Development Assignment\Task 1\Kempinski Hotel Bristol Berlin, Germany - Booking.com.html";
            var fileData = extraction.ReadHtmlFile(filePath);

            if (string.IsNullOrEmpty(fileData.Item1))
            {
                Console.WriteLine("No html content found...\nPlease restart the aplication and enter a valid file location.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nExtracting information from the html file...\n");
            var extractedData = extraction.HtmlExtractionFromStringContent(fileData.Item1);

            Console.WriteLine("Extracted json...\n");

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine(extractedData);

            Console.ResetColor();

            if (!string.IsNullOrEmpty(extractedData))
            {
                Console.WriteLine("\nSaving json output.\n");

                var outPutPath = fileData.Item2 + "\\ExtractedData.json";
                File.WriteAllText(outPutPath, extractedData);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Output is saved at\n{outPutPath}");
                Console.ResetColor();

                Console.WriteLine("\nTrying to open the output folder in explorer.");
                try
                {
                    Process.Start(fileData.Item2);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if (ex.Message.Contains("Access is denied"))
                    {
                        Console.WriteLine("Access is denied to open the folder in explorer.");
                    }
                    else
                    {
                        Console.WriteLine("Unable to open the folder.");
                    }
                    Console.ResetColor();
                }
            }

            Console.Read();
        }
    }
}
