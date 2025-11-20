using FarmXpert.Domain.Interfaces;

namespace FarmXpert.Infrastructure.Repositories;
public class FileStorageServiceRepository : IFileStorageService
{
    private readonly string _storagePath;

    public FileStorageServiceRepository(string storagePath = null)
    {
        _storagePath = storagePath ?? Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");

        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
    {
        string fullPath = Path.Combine(_storagePath, fileName);
        using var outputStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        await fileStream.CopyToAsync(outputStream);

        return fullPath;
    }

    public Task DeleteFileAsync(string fullPath)
    {
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }
}
