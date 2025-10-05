namespace FarmXpert.Domain.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName);
    Task DeleteFileAsync(string fullPath);
}

