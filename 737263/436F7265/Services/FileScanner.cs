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

        // Added IProgress<string>? with a question mark to satisfy nullable reference types
        public static async Task ScanDirectory(string path, HashAlgorithmType algo, 
            IProgress<string>? progress = null)
        {
            FoundFiles.Clear();
            _processedCount = 0;
            _totalFiles = 0;

            // Report to UI instead of Console
            progress?.Report("Starting scan... building file tree.");

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

                        // Optionally report finding files so the UI doesn't look frozen on huge folders
                        if (_totalFiles % 1000 == 0)
                        {
                            progress?.Report($"Found {_totalFiles} files so far...");
                        }
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
                progress?.Report($"[!] Critical access error in root directory: {ex.Message}");
                return;
            }

            progress?.Report($"Found {_totalFiles} files. Optimizing hash queue...");

            // Select only files that share a size with at least one other file
            var potentialDuplicates = bySize.Values.Where(g => g.Count > 1).SelectMany(g => g).ToList();

            progress?.Report($"Files queued for hashing: {potentialDuplicates.Count}");

            var options = new ParallelOptions
            {
                // Let the system decide the optimal number of threads based on CPU cores
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            // Parallel.ForEachAsync takes care of thread management 
            await Parallel.ForEachAsync(potentialDuplicates, options, async (file, ct) =>
            {
                // Passed the 'progress' variable correctly here
                await ProcessFile(file, algo, potentialDuplicates.Count, progress);
            });
        }

        private static async Task ProcessFile(string filePath, HashAlgorithmType algo,
            int totalToHash, IProgress<string>? progress)
        {
            try
            {
                // Thread-safe increment for progress tracking
                int current = Interlocked.Increment(ref _processedCount);

                // THROTTLING (Optimization): Report to UI only every 50th file or on the very last file.
                // This prevents the UI thread from freezing due to a flood of update requests.
                if (current % 10 == 0 || current == totalToHash)
                {
                    progress?.Report($"[{current}/{totalToHash}] {filePath}");
                }

                string hash = await ComputeHash(filePath, algo);

                // Add file path to the corresponding hash group securely
                FoundFiles.GetOrAdd(hash, _ => new ConcurrentBag<string>()).Add(filePath);
            }
            catch (Exception)
            {
                // Silently skip corrupted/locked files during hashing
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