using DupFin.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DupFin.Enums;
using static DupFin._737263._436F7265.Services.ChoiceService;
using HashAlgorithmType = DupFin.Enums.HashAlgorithmType;
using ScanMode = DupFin.Enums.ScanMode;


namespace DupFin
{
      class Program
      {
        static ScanMode GetScanMode()
        {
            Console.WriteLine("\nChoose type of scan");

            var modes = Enum.GetNames(typeof(ScanMode));

            for (int i = 0; i < modes.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {modes[i]}");
            }

            Console.Write($"\nChoose (1-{modes.Length}): ");
            string choice = Console.ReadLine();

            // Parsing user input and validating it
            if (int.TryParse(choice, out int index) && index >= 1 && index <= modes.Length)
            {
                return (ScanMode)Enum.Parse(typeof(ScanMode), modes[index - 1]);
            }

            // Default is Async
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Are you stupid? ok we gona use Async");
            Console.ResetColor();
            return ScanMode.Async;
        }

        // Same for hash
        static HashAlgorithmType GetHashAlgorithm()
        {
            Console.WriteLine("\nChoose type of hash");

            var algos = Enum.GetNames(typeof(HashAlgorithmType));

            for (int i = 0; i < algos.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {algos[i]}");
            }

            Console.Write($"\nChoose (1-{algos.Length}): ");
            string choice = Console.ReadLine();

            if (int.TryParse(choice, out int index) && index >= 1 && index <= algos.Length)
            {
                return (HashAlgorithmType)Enum.Parse(typeof(HashAlgorithmType), algos[index - 1]);
            }

            return HashAlgorithmType.SHA256; // Default
        }
        // Getting save path
        static string GetSavePath()
        {
            Console.Write("\nSave results to file? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y") return null;

            Console.Write("Folder (Enter for defoult): ");
            string folder = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(folder)) folder = Directory.GetCurrentDirectory();

            Console.Write("Filename (Enter for auto): ");
            string filename = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(filename))
                filename = $"DupFin_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            else if (!filename.EndsWith(".txt")) filename += ".txt";

            return Path.Combine(folder, filename);
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
                sb.AppendLine($"Total duplicate groups: {group.Value.Count}");
            }


            File.WriteAllText(filePath, sb.ToString());
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Results saved to: {filePath}");
            Console.ResetColor();
        }

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

            // Choice of scan mode and hash algorithm
            ScanMode mode = GetScanMode();
            HashAlgorithmType algo = GetHashAlgorithm();

            // Scanning
            Console.WriteLine($"\nScanning: {path}...");
            await FileScanner.ScanDirectory(path, algo);

            // Debug results
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("\nPress Enter to exit [DEBUG]");
            Console.ReadKey();
            Console.Clear();

            // Saving
            string savePath = GetSavePath();
            if (savePath != null)
                SaveResults(FileScanner.FoundFiles, savePath);

            // Results 2
            PrintResults();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadKey();
        }
      }
}