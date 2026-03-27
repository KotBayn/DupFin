using DupFin.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;


namespace DupFin
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            string mascot = @"
       <@_________________________>_._<_________________________@>      
       ||         :-._            |###|         :-._            ||                   
      | |       _.-'  '--.        |###|       _.-'  '--.        | |     
      / |     .'      _  -`\_     |] [|     .'      _  -`\_     | \     
     /  |    / .----.`_.'----'    |   |    / .----.`_.'----'    |  \    
    /`  |    ;/     `             |   |    ;/     `             |  '\    
    |   |   /_;                   |   |   /_;                   |   |
    \   |_________________________|   |_________________________|   /  


";
            Console.WriteLine(mascot);
            Console.WriteLine("*===*Welcome to DupFin 1.1*===*");

            Console.WriteLine("Choose type of scan");

            // Enter — searching in current path
            Console.WriteLine("Choose directory or press 'Enter' for current path");
            string path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path)) path = Directory.GetCurrentDirectory();

            if (!Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Directory not found! Check the path, dummy.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Scaning path: {path} ...");

            await FileScanner.ScanDirectoryAsync(path);

            Console.WriteLine("\nPress Enter to exit [DEBUG]");
            Console.ReadKey();
            Console.Clear();

            Console.Write("Save to Scan result.txt? (y/n): ");
            if (Console.ReadLine()?.ToLower() == "y")
            {
                string fileName = $"DupFin_Results_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                SaveResults(FileScanner.FoundFiles, fileName);
            }
            // Results 2
            PrintResults();
                
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadKey();
        }

        // Results 1
        static void PrintResults()
        {
            Console.WriteLine("\n*===* RESULTS *===*");
            int duplicates = 0;

            foreach (var group in FileScanner.FoundFiles)
            {
                if (group.Value.Count > 1)
                {
                    duplicates += group.Value.Count - 1;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n[!] Hash: {group.Key.Substring(0, 10)}...");
                    Console.ResetColor();

                    foreach (var f in group.Value)
                        Console.WriteLine($"    [+] {f}");
                }
            }

            Console.ForegroundColor = duplicates > 0 ? ConsoleColor.Green : ConsoleColor.White;
            Console.WriteLine($"\nTotal duplicates found:: {duplicates}");
            Console.ResetColor();
        }

        // Method to save results
        static void SaveResults(Dictionary<string, List<string>> hashes, string filePath)
        {
            // Using StringBuilder for efficiensy, especially with large results
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("*===* DupFin Scan Results *===*");
            sb.AppendLine($"Date: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
            sb.AppendLine();

            int duplicateCount = 0;

            foreach (var group in hashes)
            {
                if (group.Value.Count > 1) // Only groups with duplicates
                {
                    duplicateCount += group.Value.Count - 1;

                    // Writing the hash
                    sb.AppendLine($"[Hash: {group.Key}]");

                    // Writing the names of the files with same hash
                    foreach (string file in group.Value)
                    {
                        sb.AppendLine($"  + {file}");
                    }
                    sb.AppendLine(); // indentation
                }
            }

            // Statistics
            sb.AppendLine($"*==========================*");
            sb.AppendLine($"Total duplicate groups: {duplicateCount}");

            // Writing to file
            System.IO.File.WriteAllText(filePath, sb.ToString());

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Results saved to {filePath}");
            Console.ResetColor();
        }
    }
}