using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.SocialPost.Queries.GetSocialPostById;

public class GetSocialPostByIdQueryHandler : IRequestHandler<GetSocialPostByIdQuery, Domain.Entities.SocialPost?>
{
    private readonly ISocialPostRepository _socialPostRepository;

    public GetSocialPostByIdQueryHandler(ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }

    public async Task<Domain.Entities.SocialPost?> Handle(GetSocialPostByIdQuery request, CancellationToken cancellationToken)
    {
        return await _socialPostRepository.GetByIdAsync(request.id, cancellationToken);
    }
}
