using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DupFin.Services
{
    public static class FileScanner
    {
        // Dictionary: hash to List files
        public static Dictionary<string, List<string>> FoundFiles { get; } = new();

        // lock for thread safety
        private static readonly object _lock = new object();

        // Main method to scan directory asynchronously
        public static async Task ScanDirectoryAsync(string path)
        {
            FoundFiles.Clear(); // Crearing before new scan
            // Getting all files recursively
            // SearchOption.AllDirectories — subfolders
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            Console.WriteLine($"Found {files.Length} files. Starting async scan...");

            var tasks = new List<Task>();

            foreach (string file in files)
            {
                // Adding tasks for files
                tasks.Add(ProcessFileAsync(file));
            }
            await Task.WhenAll(tasks);
        }

        // Method to process each file: check size, compute hash, and store in dictionary
        private static async Task ProcessFileAsync(string filePath)
        {
            try
            {
                if (new FileInfo(filePath).Length == 0) return;

                // Scanning file and computing hash
                string hash = await ComputeHashAsync(filePath);

                // Writing to dictionary
                lock (_lock)
                {
                    if (!FoundFiles.ContainsKey(hash))
                        FoundFiles[hash] = new List<string>();
                        FoundFiles[hash].Add(filePath);
                }
                FoundFiles[hash].Add(filePath);
                Console.WriteLine($"[DEBUG] Thread " +
                    $"{System.Threading.Thread.CurrentThread.ManagedThreadId}: Adding {filePath} " +
                    $"to hash {hash[..8]}... Total keys in dictionary: {FoundFiles.Count}");
            }
            catch (Exception ex)
            {
                // quietly handle errors and continue
                Console.WriteLine($"[!] Error with {filePath}: {ex.Message}");
            }                        
        }
        // Method hash file content using SHA256
        private static async Task<string> ComputeHashAsync(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create()) 
            using (FileStream stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = await Task.Run(() => sha256.ComputeHash(stream));

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
    }
}