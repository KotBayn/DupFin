using DupFin.Enums;       
using DupFin.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DupFin._7465737473
{
    public class StartupTests
    {
        [Fact]
        private async Task ScanAsync()
        {
            try
            {
                // SHA256
                await FileScanner.ScanDirectory(_path, HashAlgorithmType.SHA256);

                var resultsForm = new ResultsForm();
                resultsForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
                this.Close();
            }
        }
    }
    public class HashTests
    {
        [Fact]
        public async Task ComputeHash_SameCont_SameHash()
        {
            // Arrange, making a temporary file with known content
            string tempFile = Path.GetTempFileName();
            string testContent = "DupFin make a DUP!";
            await File.WriteAllTextAsync(tempFile, testContent);

            // Act, computing hash of the same file twice
            string hash1 = await FileScanner.ComputeHash(tempFile, HashAlgorithmType.SHA256);
            string hash2 = await FileScanner.ComputeHash(tempFile, HashAlgorithmType.SHA256);

            // Assert, the hashes should be the same
            Assert.Equal(hash1, hash2);

            // Clean up
            File.Delete(tempFile);
        }

        [Fact]
        public async Task ComputeHash_DiffCont_DiffHash()
        {
            // 1. Arrange
            string file1 = Path.GetTempFileName();
            string file2 = Path.GetTempFileName();
            await File.WriteAllTextAsync(file1, "Content A");
            await File.WriteAllTextAsync(file2, "Content B");

            // 2. Act
            string hash1 = await FileScanner.ComputeHash(file1, HashAlgorithmType.SHA256);
            string hash2 = await FileScanner.ComputeHash(file2, HashAlgorithmType.SHA256);

            // 3. Assert
            Assert.NotEqual(hash1, hash2);  // Need to be different!

            // 4. Cleanup
            File.Delete(file1);
            File.Delete(file2);
        }
        [Fact]
        public async Task ComputeHash_SHA256_Returns64Characters()
        {
            // 1. Arrange
            string tempFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFile, "Test");

            // 2. Act
            string hash = await FileScanner.ComputeHash(tempFile, HashAlgorithmType.SHA256);

            // 3. Assert
            Assert.Equal(64, hash.Length);  // SHA256 = 64 hex-symbols

            // 4. Cleanup
            File.Delete(tempFile);
        }
    }
}