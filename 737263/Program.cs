using DupFin.Services;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DupFin.Enums;

// Importing ChoiceService methods directly
using static DupFin._737263._436F7265.Services.ChoiceService;

namespace DupFin
{
    class Program
    {
        // Method to print results to the console
        static void PrintResults()
        {
            Console.WriteLine("\n*===* RESULTS *===*");
            int duplicates = 0;

            // Iterating over the ConcurrentDictionary from FileScanner
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
            Console.WriteLine($"\nTotal duplicates found: {duplicates}");
            Console.ResetColor();
        }

        // Method to save results to a text file
        static void SaveResults(ConcurrentDictionary<string, ConcurrentBag<string>> hashes, string filePath)
        {
            // Using StringBuilder for efficiency with large string concatenations
            var sb = new StringBuilder();

            sb.AppendLine("*===* DupFin Scan Results *===*");
            sb.AppendLine($"Date: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
            sb.AppendLine();

            int duplicateCount = 0;

            foreach (var group in hashes)
            {
                if (group.Value.Count > 1) // Only process groups with actual duplicates
                {
                    duplicateCount += group.Value.Count - 1;

                    // Writing the truncated hash
                    sb.AppendLine($"[Hash: {group.Key}]");

                    // Writing the paths of the identical files
                    foreach (string file in group.Value)
                    {
                        sb.AppendLine($"  + {file}");
                    }
                    sb.AppendLine(); // Empty line for readability
                }
            }

            // Calculating total groups that contain duplicates
            int totalGroups = hashes.Count(g => g.Value.Count > 1);
            sb.AppendLine($"Total duplicate groups: {totalGroups}");

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
            Console.WriteLine("*===* Welcome to DupFin 1.1 *===*");

            // Prompt user for directory path
            Console.WriteLine("Choose directory or press 'Enter' for current path:");
            string? path = Console.ReadLine();

            // Fallback to current directory if input is empty
            if (string.IsNullOrWhiteSpace(path))
                path = Directory.GetCurrentDirectory();

            // Validate directory existence
            if (!Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Directory not found! Check the path, dummy.");
                Console.ResetColor();
                return;
            }

            // Get hash algorithm using ChoiceService
            HashAlgorithmType algo = GetHashAlgorithm();

            // Start the scanning process
            Console.WriteLine($"\nScanning: {path}...");
            await FileScanner.ScanDirectory(path, algo);

            // Pause for debugging before saving/printing
            Console.WriteLine(new string('-', 50));
            Console.WriteLine("\nPress Enter to view results [DEBUG]");
            Console.ReadKey();
            Console.Clear();

            // Handle file saving via ChoiceService
            string? savePath = GetSavePath();
            if (savePath != null)
            {
                SaveResults(FileScanner.FoundFiles, savePath);
            }

            // Print results to the console
            PrintResults();

            // Keep console open until user exits
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadKey();
        }
    }
}