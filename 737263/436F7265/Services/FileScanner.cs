using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using DupFin.Enums;

namespace DupFin.Services
{
    public static class FileScanner
    {
        // Thread-safe dictionary storing hashes as keys and thread-safe collections of file paths as values
        public static ConcurrentDictionary<string, ConcurrentBag<string>> FoundFiles { get; } = new();

        private static int _processedCount = 0;
        private static int _totalFiles = 0;

        public static async Task ScanDirectory(string path, HashAlgorithmType algo)
        {
            FoundFiles.Clear();
            _processedCount = 0;
            _totalFiles = 0;

            Console.WriteLine("Starting scan...");

            // Initial grouping by file size to quickly filter out unique files
            var bySize = new Dictionary<long, List<string>>();

            try
            {
                // EnumerateFiles avoids loading all files into memory at once
                foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var size = new FileInfo(file).Length;
                        if (size == 0) continue; // Skip empty files

                        if (!bySize.ContainsKey(size)) bySize[size] = new List<string>();
                        bySize[size].Add(file);

                        _totalFiles++;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Silently ignore restricted system/hidden files
                    }
                    catch (Exception)
                    {
                        // Ignore other read exceptions to prevent scan termination
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Critical access error in root directory: {ex.Message}");
                return;
            }

            Console.WriteLine($"Found {_totalFiles} files. Optimizing hash queue...");

            // Select only files that share a size with at least one other file
            var potentialDuplicates = bySize.Values.Where(g => g.Count > 1).SelectMany(g => g).ToList();

            Console.WriteLine($"Files queued for hashing: {potentialDuplicates.Count} (Skipped {_totalFiles - potentialDuplicates.Count} unique files)");

            var options = new ParallelOptions
            {
                // 8 cores - 8 threads, 16 cores - 16 threads, etc.
                // Let the system decide the optimal number of threads.
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            // Parallel.ForEachAsync takes care of thread management 
            await Parallel.ForEachAsync(potentialDuplicates, options, async (file, ct) =>
            {
                // ct - its CancellationToken, just in case
                await ProcessFile(file, algo, potentialDuplicates.Count);
            });
        }

        private static async Task ProcessFile(string filePath, HashAlgorithmType algo, int totalToHash)
        {
            try
            {
                // Thread-safe increment for progress tracking
                int current = Interlocked.Increment(ref _processedCount);
                Console.WriteLine($"[{current}/{totalToHash}] Hashing: {filePath}");

                string hash = await ComputeHash(filePath, algo);

                // Add file path to the corresponding hash group securely
                FoundFiles.GetOrAdd(hash, _ => new ConcurrentBag<string>()).Add(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error processing {filePath}: {ex.Message}");
            }
        }

        public static HashAlgorithm CreateHasher(HashAlgorithmType algo)
        {
            return algo switch
            {
                HashAlgorithmType.MD5 => MD5.Create(),
                HashAlgorithmType.SHA256 => SHA256.Create(),
                HashAlgorithmType.SHA512 => SHA512.Create(),
                _ => SHA256.Create() // Default fallback
            };
        }

        public static async Task<string> ComputeHash(string filePath, HashAlgorithmType algo)
        {
            using HashAlgorithm hasher = CreateHasher(algo);

            // FileShare.Read ensures we can read files currently opened by other processes
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            byte[] hashBytes = await Task.Run(() => hasher.ComputeHash(stream));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}