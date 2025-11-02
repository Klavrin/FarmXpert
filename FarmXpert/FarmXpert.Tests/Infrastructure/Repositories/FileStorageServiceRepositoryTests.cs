using FarmXpert.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
using System.Text;
using Xunit;

namespace FarmXpert.Tests.Infrastructure.Repositories;

public class FileStorageServiceRepositoryTests : IDisposable
{
    private readonly string _tempDir;

    public FileStorageServiceRepositoryTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "FileStorageTests");
        if (!Directory.Exists(_tempDir))
            Directory.CreateDirectory(_tempDir);
    }

    private FileStorageServiceRepository CreateRepository()
    {
        return new FileStorageServiceRepository(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, recursive: true);
        }
    }

    [Fact]
    public void Constructor_ShouldCreateDirectory_WhenItDoesNotExist()
    {
        // Arrange
        var testDir = Path.Combine(_tempDir, "ConstructorTest");
        if (Directory.Exists(testDir))
            Directory.Delete(testDir, recursive : true);

        // Act
        var repo = new FileStorageServiceRepository(testDir);

        // Assert
        Directory.Exists(testDir).Should().BeTrue();
    }

    [Fact]
    public async Task SaveFileAsync_ShouldSaveFileToDisk()
    {
        // Arrange
        var repo = CreateRepository();
        var fileName = "test_create.txt";
        var content = "Hello, FarmXpert!";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

        // Act
        var path = await repo.SaveFileAsync(stream, fileName);

        // Assert
        File.Exists(path).Should().BeTrue();
        var savedContent = await File.ReadAllTextAsync(path);
        savedContent.Should().Be(content);
    }

    [Fact]
    public async Task DeleteFileAsync_ShouldDeleteFileFromDisk()
    {
        // Arrange
        var repo = CreateRepository();
        var fileName = "test_delete.txt";
        var content = "To be deleted";
        var filePath = Path.Combine(_tempDir, fileName);
        await File.WriteAllTextAsync(filePath, content);

        // Act
        await repo.DeleteFileAsync(filePath);

        // Assert
        File.Exists(filePath).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteFileAsync_ShouldNotThrow_WhenFileDoesNotExist()
    {
        // Arrange
        var repo = CreateRepository();
        var nonExistentPath = Path.Combine(_tempDir, "does_not_exist.txt");

        // Act
        Func<Task> act = async () => await repo.DeleteFileAsync(nonExistentPath);

        // Assert
        await act.Should().NotThrowAsync();
    }
}
