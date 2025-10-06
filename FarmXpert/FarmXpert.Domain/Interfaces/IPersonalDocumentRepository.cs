using System.Reflection.Metadata;
using FarmXpert.Domain.Entities;

namespace FarmXpert.Domain.Interfaces;

public interface IPersonalDocumentRepository
{
    Task CreateAsync(PersonalDocument document, CancellationToken cancellationToken = default);
    Task<IEnumerable<PersonalDocument>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default);
    Task<PersonalDocument?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(PersonalDocument document, CancellationToken cancellationToken = default);
    Task DeleteAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
}