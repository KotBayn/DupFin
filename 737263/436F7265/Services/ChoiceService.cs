using DupFin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DupFin;
using DupFin.Enums;
using HashAlgorithmType = DupFin.Enums.HashAlgorithmType;


namespace DupFin._737263._436F7265.Services
{
    public class ChoiceService
    {
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

        // Method to get the save path from the user
        // It ONLY generates the path, it DOES NOT save the file
        public static string? GetSavePath()
        {
            Console.Write("\nSave to file? (y/n): ");
            if (Console.ReadLine()?.ToLower() != "y") return null;

            // Get path to Desktop
            string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Console.Write("Choose folder (Enter for default): ");
            string? folder = Console.ReadLine();

            // If user pressed Enter, use Desktop/DupFin
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = Path.Combine(deskPath, "DupFin");
            }

            // Create folder if it does not exist
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // Auto filename with timestamp
            string fileName = $"DupFin_Results_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            // Return the full combined path to Program.cs
            return Path.Combine(folder, fileName);
        }
    }
}
