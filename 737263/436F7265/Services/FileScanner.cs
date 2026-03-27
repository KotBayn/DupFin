using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DupFin.Enums;
using HashAlgorithmType = DupFin.Enums.HashAlgorithmType;


namespace DupFin.Services
{
    public static class FileScanner
    {
        // Dictionary: hash to List files
        public static Dictionary<string, List<string>> FoundFiles { get; } = new();

        // lock for thread safety
        private static readonly object _lock = new object();

        // Main method to scan directory asynchronously
        public static async Task ScanDirectory(string path, ScanMode mode, HashAlgorithm algo)
        {
            FoundFiles.Clear(); // Crearing before new scan
                                // Getting all files recursively
                                // SearchOption.AllDirectories — subfolders
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            Console.WriteLine($"Found {files.Length} files. Starting scan...");


            // Filtering by size first to minimize hashing
            var bySize = new Dictionary<long, List<string>>();

            foreach (var file in files)
            {
                try
                {
                    var size = new FileInfo(file).Length;
                    if (size == 0) continue;

                    if (!bySize.ContainsKey(size))
                        bySize[size] = new List<string>();

                    bySize[size].Add(file);
                }
                catch { /* Skipping errors */ }
            }

            Console.WriteLine($"Optimize {files.Length} files => {bySize.Count} size groups");

            // Hashing only files that potential duplicates
            var potentialDuplicates = bySize.Values.Where(g => g.Count > 1).SelectMany(g => g).ToList();
            Console.WriteLine($"Optimtze hashing {potentialDuplicates.Count} files " +
                $"(skipped {files.Length - potentialDuplicates.Count})");

            var tasks = new List<Task>();
            foreach (var file in potentialDuplicates)
            {
                tasks.Add(ProcessFileAsync(file, algo));
            }

            await Task.WhenAll(tasks);
        }

        // One file processing
        private static async Task ProcessFileAsync(string filePath, HashAlgorithmType algo)
        {
            try
            {
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
            // quietly handle errors and continue
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error with {filePath}: {ex.Message}");
            }
        }

        // Method to create hasher based on choice
        private static System.Security.Cryptography.HashAlgorithm CreateHasher(HashAlgorithmType algo)
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
    
            /*switch (mode)
            {
                case ScanMode.Async:
                    await ScanAsync(path, algo);
                    break;
                case ScanMode.Parallel:
                    ScanParallel(path, algo);
                    break;
                case ScanMode.Sync:
                    ScanSync(path, algo);
                    break;
            */

        // Method hash file content
        private static async Task<string> ComputeHash0(string filePath, HashAlgorithmType algo)
        {
            using System.Security.Cryptography.HashAlgorithm hasher = CreateHasher(algo); // ← Общий тип HashAlgorithm
            using FileStream stream = File.OpenRead(filePath);

            byte[] hashBytes = await Task.Run(() => hasher.ComputeHash(stream));

            // Convert bytes in hex-string
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
