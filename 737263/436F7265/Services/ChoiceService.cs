using DupFin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DupFin.Enums;


namespace DupFin._737263._436F7265.Services
{
    using HashAlgorithm = DupFin._737263._436F7265.Services.ChoiceService.HashAlgorithm;
    internal class ChoiceService
    {


        // Method to get user choices for scan mode
        static ScanMode GetScanMode()
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
        static HashAlgorithm GetHashAlgorithm()
        {
            Console.WriteLine("\n+-+HASH ALGORITHM+-+");
            Console.WriteLine("1. MD5");
            Console.WriteLine("2. SHA256");
            Console.WriteLine("3. SHA512");
            Console.Write("Choose: ");

            return Console.ReadLine() switch
            {
                "1" => HashAlgorithm.MD5,
                "2" => HashAlgorithm.SHA256,
                "3" => HashAlgorithm.SHA512,
                _ => HashAlgorithm.SHA256 // default
            };
        }

        // Method to get save path
        static string GetSavePath()
        {
            Console.Write("\nSave to file? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y") return null;

            // Get path to My Documents
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            Console.Write("Choose folder (Enter for defoult): ");
            string folder = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(folder)) folder = docPath;
                        
            // Make subfolder for files
            string folderName = "DupFin";
            string path = Path.Combine(docPath, folderName);

            // Create folder if not exists
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // 3. Формируем полное имя файла
            string fileName = $"DupFin_Results_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = Path.Combine(path, fileName);
            SaveResults(FileScanner.FoundFiles, fileName);

            return Path.Combine(folder, fileName);
        }
    }
}
