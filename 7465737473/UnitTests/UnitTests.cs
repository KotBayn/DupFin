using DupFin.Enums;
using DupFin.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DupFin.Tests
{
    public class HashTests
    {
        [Fact]
        public async Task ComputeHash_SameCont_SameHash()
        {
            // 1. Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                string testContent = "DupFin make a DUP!";
                await File.WriteAllTextAsync(tempFile, testContent);

                // 2. Act
                string hash1 = await FileScanner.ComputeHash(tempFile, HashAlgorithmType.SHA256);
                string hash2 = await FileScanner.ComputeHash(tempFile, HashAlgorithmType.SHA256);

                // 3. Assert
                Assert.Equal(hash1, hash2);
            }
            finally
            {
                // 4. Cleanup (Guaranteed to run even if Assert fails)
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        [Fact]
        public async Task ComputeHash_DiffCont_DiffHash()
        {
            // 1. Arrange
            string file1 = Path.GetTempFileName();
            string file2 = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(file1, "Content A");
                await File.WriteAllTextAsync(file2, "Content B");

                // 2. Act
                string hash1 = await FileScanner.ComputeHash(file1, HashAlgorithmType.SHA256);
                string hash2 = await FileScanner.ComputeHash(file2, HashAlgorithmType.SHA256);

                // 3. Assert
                Assert.NotEqual(hash1, hash2);
            }
            finally
            {
                // 4. Cleanup
                if (File.Exists(file1)) File.Delete(file1);
                if (File.Exists(file2)) File.Delete(file2);
            }
        }

        [Fact]
        public async Task ComputeHash_SHA256_Returns64Characters()
        {
            // 1. Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(tempFile, "Test");

                // 2. Act
                string hash = await FileScanner.ComputeHash(tempFile, HashAlgorithmType.SHA256);

                // 3. Assert (SHA256 is always 64 hex characters)
                Assert.Equal(64, hash.Length);
            }
            finally
            {
                // 4. Cleanup
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }
        public class ScannerTests
        {
            [Fact]
            public async Task ScanDirectory_WithDuplicates_FindsCorrectGroups()
            {
                // 1. Arrange, making a unique temp directory for test run
                string tempDir = Path.Combine(Path.GetTempPath(), "DupFin_TestDir_" + Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDir);

                try
                {
                    // Create 2 duplicate files (same content, same size)
                    string dup1 = Path.Combine(tempDir, "dup1.txt");
                    string dup2 = Path.Combine(tempDir, "dup2.txt");
                    await File.WriteAllTextAsync(dup1, "I am a duplicate!");
                    await File.WriteAllTextAsync(dup2, "I am a duplicate!");

                    // Create 1 unique file (different content, same size)
                    string unique = Path.Combine(tempDir, "unique.txt");
                    await File.WriteAllTextAsync(unique, "I am totally unique and different.");

                    // 2. Act - Scan the directory
                    await FileScanner.ScanDirectory(tempDir, HashAlgorithmType.MD5);

                    // 3. Assert - check the results
                    var results = FileScanner.FoundFiles;

                    // Need to find the group that contains our duplicates
                    Assert.Single(results);

                    // In group, need to be exactly 2 files (the duplicates)
                    var firstGroup = results.Values.First();
                    Assert.Equal(2, firstGroup.Count);
                }
                finally
                {
                    // 4. Cleanup
                    if (Directory.Exists(tempDir))
                    {
                        Directory.Delete(tempDir, true);
                    }
                }
            }
        }
    }
}