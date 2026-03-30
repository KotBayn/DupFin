using DupFin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DupFin.Enums;
using HashAlgorithmType = DupFin.Enums.HashAlgorithmType;


namespace DupFin._737263._436F7265.Services
{
    public class ChoiceService
    {
        // Method to get user choices for scan mode
        public static ScanMode GetScanMode()
        {
            Console.WriteLine("\n+-+SCAN MODE+-+");
            Console.WriteLine("1. Async (I/O bound)");
            Console.WriteLine("2. Parallel (CPU bound)");
            Console.WriteLine("3. Sync (simple)");
            Console.WriteLine("4. Bidirectional");
            Console.Write("Choose: ");

            return Console.ReadLine() switch
            {
                "1" => ScanMode.Async,
                "2" => ScanMode.Parallel,
                "3" => ScanMode.Sync,
                "4" => ScanMode.Bidirectional,
                 _ => ScanMode.Async // default
            };
        }

        // Method to get choice for hash algorithm
        public static HashAlgorithmType GetHashAlgorithm()
        {
            Console.WriteLine("\n+-+HASH ALGORITHM+-+");
            Console.WriteLine("1. MD5");
            Console.WriteLine("2. SHA256");
            Console.WriteLine("3. SHA512");
            Console.Write("Choose: ");

            return Console.ReadLine() switch
            {
                "1" => HashAlgorithmType.MD5,
                "2" => HashAlgorithmType.SHA256,
                "3" => HashAlgorithmType.SHA512,
                _ => HashAlgorithmType.SHA256 // default
            };
        }

        // Method to get save path
        public static string GetSavePath()
        {
            Console.Write("\nSave to file? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y") return null;

            // Get path to Desktop
            string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Console.Write("Choose folder (Enter for defoult): ");
            string folder = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(folder)) folder = deskPath;
                        
            // Make subfolder for files
            string folderName = "DupFin";
            string path = Path.Combine(deskPath, folderName);

            // Create folder if not exists
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Auto filename with timestamp
            string fileName = $"DupFin_Results_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = Path.Combine(path, fileName);
            SaveResults(FileScanner.FoundFiles, fileName);

            return Path.Combine(folder, fileName);
        }
        // Method to save results
        public static void SaveResults(Dictionary<string, List<string>> hashes, string filePath)
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
    }
}
