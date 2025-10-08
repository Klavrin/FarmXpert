using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmXpert.Domain.Interfaces;

public interface ISocialPostRepository
{
    Task CreateAsync(Entities.SocialPost post, CancellationToken cancellationToken = default);
    Task<Entities.SocialPost?> GetByIdAsync(string OwnerId, Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Entities.SocialPost>> GetAllAsync(string OwnerId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Entities.SocialPost post, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
