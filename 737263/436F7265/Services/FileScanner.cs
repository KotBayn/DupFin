using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using DupFin.Enums;
using System.IO.Hashing;

namespace DupFin.Services
{
    public static class FileScanner
    {
        public static ConcurrentDictionary<string, ConcurrentBag<string>> FoundFiles { get; } = new();

        private static int _processedCount = 0;

        public static async Task ScanDirectory(string path, HashAlgorithmType algo, bool matchName, IProgress<string>? progress = null)
        {
            FoundFiles.Clear();
            _processedCount = 0;

            progress?.Report("Stage 1: Building file tree and filtering by size...");

            var bySize = new Dictionary<long, List<string>>();
            int totalScanned = 0;

            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var size = new FileInfo(file).Length;
                        if (size == 0) continue;

                        if (!bySize.ContainsKey(size)) bySize[size] = new List<string>();
                        bySize[size].Add(file);

                        totalScanned++;
                        if (totalScanned % 5000 == 0) progress?.Report($"Found {totalScanned} files...");
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                progress?.Report($"[!] Critical access error: {ex.Message}");
                return;
            }

            // Select groups of files that have the same size
            var sameSizeGroups = bySize.Values.Where(g => g.Count > 1).ToList();
            var filesForPartialHash = sameSizeGroups.SelectMany(g => g).ToList();

            if (filesForPartialHash.Count == 0)
            {
                progress?.Report("No duplicates found based on file size.");
                return;
            }

            progress?.Report($"Stage 2: Fast Pass (Partial Hashing) for {filesForPartialHash.Count} files...");

            // Dictionary for groups by partial hash
            var partialHashes = new ConcurrentDictionary<string, ConcurrentBag<string>>();
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };

            await Parallel.ForEachAsync(filesForPartialHash, options, async (file, ct) =>
            {
                try
                {
                    string pHash = await ComputePartialHash(file, algo);
                    long size = new FileInfo(file).Length;
                    string compositeKey = $"{size}_{pHash}";

                    partialHashes.GetOrAdd(compositeKey, _ => new ConcurrentBag<string>()).Add(file);
                }
                catch { }
            });

            // Select files with matching size and partial hash
            var filesForFullHash = partialHashes.Values.Where(g => g.Count > 1).SelectMany(g => g).ToList();

            if (filesForFullHash.Count == 0)
            {
                progress?.Report("No duplicates found after partial hash analysis.");
                return;
            }

            progress?.Report($"Stage 3: Deep Scan (Full Hashing) for {filesForFullHash.Count} files...");
            _processedCount = 0;

            await Parallel.ForEachAsync(filesForFullHash, options, async (file, ct) =>
            {
                await ProcessFullHash(file, algo, filesForFullHash.Count, matchName, progress);
            });
        }

        private static async Task ProcessFullHash(string filePath, HashAlgorithmType algo, int totalToHash, bool matchName, IProgress<string>? progress)
        {
            try
            {
                int current = Interlocked.Increment(ref _processedCount);
                if (current % 5 == 0 || current == totalToHash)
                {
                    progress?.Report($"Hashing [{current}/{totalToHash}] {Path.GetFileName(filePath)}");
                }

                string hash = await ComputeFullHash(filePath, algo);
                string dictionaryKey = matchName ? $"{hash}_{Path.GetFileName(filePath)}" : hash;
                FoundFiles.GetOrAdd(dictionaryKey, _ => new ConcurrentBag<string>()).Add(filePath);
            }
            catch { }
        }

        public static HashAlgorithm CreateHasher(HashAlgorithmType algo)
        {
            return algo switch
            {
                HashAlgorithmType.MD5 => MD5.Create(),
                HashAlgorithmType.SHA256 => SHA256.Create(),
                HashAlgorithmType.SHA512 => SHA512.Create(),
                _ => SHA256.Create()
            };
        }

        // Read first 64 KB
        private static async Task<string> ComputePartialHash(string filePath, HashAlgorithmType algo)
        {
            byte[] buffer = new byte[65536];
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length, true);
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            if (algo == HashAlgorithmType.XxHash64)
            {
                var xxHash = new XxHash64();
                xxHash.Append(new ReadOnlySpan<byte>(buffer, 0, bytesRead));
                byte[] hashBytes = xxHash.GetCurrentHash();
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            using HashAlgorithm cryptoHasher = CreateHasher(algo);
            byte[] legacyHashBytes = cryptoHasher.ComputeHash(buffer, 0, bytesRead);
            return BitConverter.ToString(legacyHashBytes).Replace("-", "").ToLower();
        }

        // Whole file hashing
        public static async Task<string> ComputeFullHash(string filePath, HashAlgorithmType algo)
        {
            int bufferSize = 1048576; // 1 MB
            using FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true);

            if (algo == HashAlgorithmType.XxHash64)
            {
                var xxHash = new XxHash64();
                byte[] buffer = new byte[bufferSize];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    xxHash.Append(new ReadOnlySpan<byte>(buffer, 0, bytesRead));
                }

                byte[] hashBytes = xxHash.GetCurrentHash();
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

            using HashAlgorithm cryptoHasher = CreateHasher(algo);
            byte[] legacyHashBytes = await Task.Run(() => cryptoHasher.ComputeHash(stream));
            return BitConverter.ToString(legacyHashBytes).Replace("-", "").ToLower();
        }
    }
}