using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DupFin
{
    class Program
    {
        // Dictionary for result.
        // Key = hash, value = list of paths for hash
        private static Dictionary<string, List<string>> fileHashes = new Dictionary<string, List<string>>();
        private static readonly object _lock = new object();
        static async Task Main(string[] args)
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
            Console.WriteLine("Welcome to DupFin");
            // Enter — searching in current path
            Console.WriteLine("Choose directory or press 'Enter' for current path");
            string path = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(path)) path = Directory.GetCurrentDirectory();

            if (!Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Directory not found! Check the path, dummy.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine($"Scaning path: {path} ...");

            // Getting all files recursively
            // SearchOption.AllDirectories — subfolders
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            var tasks = new List<Task>();


            foreach (string file in files)
            {
                tasks.Add(ProcessFileAsync(file));
            }

            // Waiting to ALL end
            await Task.WhenAll(tasks);

            Console.WriteLine("\nPress Enter to exit [DEBUG]");
            Console.ReadKey();
            Console.Clear();

            // Results 2
            PrintResults();

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadKey();
        }

        // asynchronous single file processing method 
        static async Task ProcessFileAsync(string filePath)
        {

            try
            {
                // Skipping empty files
                var info = new FileInfo(filePath);
                if (info.Length == 0) return;

                // Waiting for calc Hash
                string hash = await GetFileHashAsync(filePath);

                lock (_lock)
                {
                    if (!fileHashes.ContainsKey(hash))
                        fileHashes[hash] = new List<string>();

                    fileHashes[hash].Add(filePath);
                    Console.WriteLine($"[DEBUG] Thread " +
                        $"{System.Threading.Thread.CurrentThread.ManagedThreadId}: Adding {filePath} " +
                        $"to hash {hash[..8]}... Total keys in dictionary: {fileHashes.Count}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Skipping {filePath}: {ex.Message}");
            }
        }

        // Asynchronous file hashing
        static async Task<string> GetFileHashAsync(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            using (FileStream stream = File.OpenRead(filePath))
            {
                // Task.Run offloads computation to a thread from the pool
                // await releases the current thread while we wait for the result
                byte[] hashBytes = await Task.Run(() => sha256.ComputeHash(stream));

                // convert bytes in string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }
        
        // Results 1
        static void PrintResults()
        {
            Console.WriteLine("\n=== RESULTS ===");
            int duplicates = 0;

            foreach (var group in fileHashes)
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
            Console.WriteLine($"\nTotal duplicates found:: {duplicates}");
            Console.ResetColor();
        }
    }
}