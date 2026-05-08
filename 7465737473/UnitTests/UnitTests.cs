using DupFin.Enums;
using DupFin.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DupFin.Tests
{
    public class HashTests
    {
        [Fact]
        public async Task ComputeFullHash_SameContent_ReturnsSameHash()
        {
            // 1. Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                string testContent = "DupFin makes finding duplicates blazing fast!";
                await File.WriteAllTextAsync(tempFile, testContent);

                // 2. Act (Using the new ComputeFullHash method)
                string hash1 = await FileScanner.ComputeFullHash(tempFile, HashAlgorithmType.SHA256);
                string hash2 = await FileScanner.ComputeFullHash(tempFile, HashAlgorithmType.SHA256);

                // 3. Assert
                Assert.Equal(hash1, hash2);
            }
            finally
            {
                // 4. Cleanup
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        [Fact]
        public async Task ComputeFullHash_DifferentContent_ReturnsDifferentHashes()
        {
            // 1. Arrange
            string file1 = Path.GetTempFileName();
            string file2 = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(file1, "Content A");
                await File.WriteAllTextAsync(file2, "Content B");

                // 2. Act
                string hash1 = await FileScanner.ComputeFullHash(file1, HashAlgorithmType.SHA256);
                string hash2 = await FileScanner.ComputeFullHash(file2, HashAlgorithmType.SHA256);

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
        public async Task ComputeFullHash_XxHash64_HardwareAccelerationWorks()
        {
            // 1. Arrange
            string tempFile = Path.GetTempFileName();
            try
            {
                await File.WriteAllTextAsync(tempFile, "Testing hardware acceleration");

                // 2. Act - testing our new high-speed algorithm
                string hash = await FileScanner.ComputeFullHash(tempFile, HashAlgorithmType.XxHash64);

                // 3. Assert - XxHash64 produces a 16-character hex string (64 bits)
                Assert.NotNull(hash);
                Assert.Equal(16, hash.Length);
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }
    }

    public class ScannerTests
    {
        [Fact]
        public async Task ScanDirectory_MatchNameFalse_FindsDuplicatesWithDifferentNames()
        {
            // 1. Arrange: Create a temp directory
            string tempDir = Path.Combine(Path.GetTempPath(), "DupFin_TestDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                // Create 2 duplicate files but with DIFFERENT names
                string dup1 = Path.Combine(tempDir, "document_final.txt");
                string dup2 = Path.Combine(tempDir, "document_copy.txt");
                await File.WriteAllTextAsync(dup1, "Identical Content");
                await File.WriteAllTextAsync(dup2, "Identical Content");

                // 2. Act: Scan with matchName = FALSE
                await FileScanner.ScanDirectory(tempDir, HashAlgorithmType.MD5, matchName: false);

                // 3. Assert
                var results = FileScanner.FoundFiles;

                // We expect 1 group containing our 2 files because content is the same and we ignore names
                var validGroups = results.Values.Where(bag => bag.Count > 1).ToList();
                Assert.Single(validGroups);
                Assert.Equal(2, validGroups.First().Count);
            }
            finally
            {
                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public async Task ScanDirectory_MatchNameTrue_SeparatesDuplicatesWithDifferentNames()
        {
            // 1. Arrange
            string tempDir = Path.Combine(Path.GetTempPath(), "DupFin_TestDir_" + Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            try
            {
                // Create 2 duplicate files with DIFFERENT names
                string dup1 = Path.Combine(tempDir, "photo1.jpg");
                string dup2 = Path.Combine(tempDir, "photo2.jpg");
                await File.WriteAllTextAsync(dup1, "Fake Image Data");
                await File.WriteAllTextAsync(dup2, "Fake Image Data");

                // 2. Act: Scan with matchName = TRUE (Strict Mode)
                await FileScanner.ScanDirectory(tempDir, HashAlgorithmType.XxHash64, matchName: true);

                // 3. Assert
                var results = FileScanner.FoundFiles;

                // Because names are different, they should be placed in separate hash buckets
                // Meaning NO group should have more than 1 file
                var validGroups = results.Values.Where(bag => bag.Count > 1).ToList();

                Assert.Empty(validGroups); 
            }
            finally
            {
                if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
            }
        }
    }
}