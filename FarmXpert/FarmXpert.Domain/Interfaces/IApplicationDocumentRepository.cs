using FarmXpert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Domain.Interfaces;
public interface IApplicationDocumentRepository
{
    Task CreateAsync(ApplicationDocument document, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApplicationDocument>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default);
    Task<ApplicationDocument?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(ApplicationDocument document, CancellationToken cancellationToken = default);
    Task DeleteAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
}
