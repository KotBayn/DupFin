using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography; // Нужно для вычисления хешей (MD5/SHA)
using System.Text;

namespace DupFin
{
    class Program
    {
        static void Main(string[] args)
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

            // Dictionary for result.
            // Key = hash, value = list of paths for hash
            Dictionary<string, List<string>> fileHashes = new Dictionary<string, List<string>>();

            // Getting all files recursively
            // SearchOption.AllDirectories — subfolders
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                try
                {
                    // Skipping files that may be busy or unavailable
                    if (new FileInfo(file).Length == 0) continue;

                    // finding hash, if exist double - add to list
                    string hash = GetFileHash(file);

                    if (fileHashes.ContainsKey(hash))
                        fileHashes[hash].Add(file);
                    else
                        fileHashes[hash] = new List<string> { file };
                }
                catch (Exception ex)
                {
                    // delited file
                    Console.WriteLine($" Skipping file {file}: {ex.Message}");
                }
            }

            // Results
            Console.WriteLine("\nResults:");
            int duplicateCount = 0;

            foreach (var group in fileHashes)
            {
                // Only groups where files more then 1
                if (group.Value.Count > 1)
                {
                    duplicateCount += group.Value.Count - 1;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\nFind doubles (Hash: {group.Key.Substring(0, 10)}...):");
                    Console.ResetColor();

                    foreach (string filePath in group.Value)
                        Console.WriteLine($"  [+] {filePath}");
                }
            }

            if (duplicateCount == 0)
                Console.WriteLine("No duplicates.");
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nFound doubles: {duplicateCount}");
                Console.ResetColor();
            }

            Console.WriteLine("\n Press 'Enter' to exit");
            Console.ReadKey();
        }

        // Method for MD5 hash
        static string GetFileHash(string filePath)
        {
            using (MD5 md5 = MD5.Create()) // Create a hashing object
            {
                using (FileStream stream = File.OpenRead(filePath)) // Opening a reading stream
                {
                    // Calculate the hash of a file's bytes
                    byte[] hashBytes = md5.ComputeHash(stream);

                    // Bytes to string (hex-format)
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                        sb.Append(b.ToString("x2")); // "x2" = 2 symbols in hex

                    return sb.ToString();
                }
            }
        }
    }
}