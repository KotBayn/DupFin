using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DupFin.Services;


namespace DupFin
{
    class Program
    {
        static async Task Main(string[] args)
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

            // Results 2
            PrintResults();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadKey();

            // Waiting to ALL end
               
        }
        
        // Results 1
        static void PrintResults()
        {
            Console.WriteLine("\n=== RESULTS ===");
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
    }
}