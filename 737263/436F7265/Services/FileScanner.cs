using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DupFin.Enums;
using HashAlgorithmType = DupFin.Enums.HashAlgorithmType;
using ScanMode = DupFin.Enums.ScanMode;
using DupFin._737263._436F7265.Services;


namespace DupFin.Services
{
    public static class FileScanner
    {

        // Dictionary: hash to List files
        public static Dictionary<string, List<string>> FoundFiles { get; } = new();

        // lock for thread safety
        private static readonly object _lock = new object();
        // Counters for progress
        private static int _processedCount = 0;
        private static int _totalFiles = 0;
        // Main method to scan directory
        public static async Task ScanDirectory(string path, HashAlgorithmType algo)
        {
            FoundFiles.Clear();
            _processedCount = 0;
            // Crearing before new scan
            // Getting all files recursively
            // SearchOption.AllDirectories — subfolders

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            _totalFiles = files.Length;
            Console.WriteLine($"Found {files.Length} files, starting scan...");

            // filtering by size
            var bySize = new Dictionary<long, List<string>>();
            foreach (var file in files)
            {
                try
                {
                    var size = new FileInfo(file).Length;
                    if (size == 0) continue;
                    if (!bySize.ContainsKey(size)) bySize[size] = new List<string>();
                    bySize[size].Add(file);
                }
                catch { }
            }

            Console.WriteLine($"Optimize {files.Length} files => {bySize.Count} size groups");

            // Hashing only files that potential duplicates
            var potentialDuplicates = bySize.Values.Where(g => g.Count > 1).SelectMany(g => g).ToList();
            Console.WriteLine($"Optimtze hashing {potentialDuplicates.Count} files " +
                $"(skipped {files.Length - potentialDuplicates.Count})");

            var tasks = new List<Task>();
            foreach (var file in potentialDuplicates)
            {
                tasks.Add(ProcessFile(file, algo));
            }

            await Task.WhenAll(tasks);
        }
        // One file processing
        private static async Task ProcessFile(string filePath, HashAlgorithmType algo)
        {
            try
            {
                lock (_lock)
                {
                    _processedCount++;
                    Console.WriteLine($"[{_processedCount}/{_totalFiles}] {filePath}");
                }
                // Scanning file and computing hash
                string hash = await ComputeHash(filePath, algo);
                // Writing to dictionary
                lock (_lock)
                {
                    if (!FoundFiles.ContainsKey(hash))
                        FoundFiles[hash] = new List<string>();
                    FoundFiles[hash].Add(filePath);

                }
            }
            catch (Exception ex) // quietly handle errors and continue
            {
                Console.WriteLine($"[!] Error: {filePath} - {ex.Message}");
            }
        }
        // Method to create hasher based on choice
        private static HashAlgorithm CreateHasher(HashAlgorithmType algo)
            {
                return algo switch
                {
                    HashAlgorithmType.MD5 => MD5.Create(),
                    HashAlgorithmType.SHA256 => SHA256.Create(),
                    HashAlgorithmType.SHA512 => SHA512.Create(),
                    _ => SHA256.Create()
                };
        }
        // Hashing
        private static async Task<string> ComputeHash(string filePath, HashAlgorithmType algo)
        {
                using HashAlgorithm hasher = CreateHasher(algo);
                using FileStream stream = File.OpenRead(filePath);
                byte[] hashBytes = await Task.Run(() => hasher.ComputeHash(stream));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}

 
