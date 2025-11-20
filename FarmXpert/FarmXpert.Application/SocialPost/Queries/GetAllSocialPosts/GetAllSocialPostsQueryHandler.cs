using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.SocialPost.Queries.GetAllSocialPosts;

public class GetAllSocialPostsQueryHandler : IRequestHandler<GetAllSocialPostsQuery, List<FarmXpert.Domain.Entities.SocialPost>>
{
    private readonly ISocialPostRepository _socialPostRepository;
    public GetAllSocialPostsQueryHandler(ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }
    public async Task<List<FarmXpert.Domain.Entities.SocialPost>> Handle(GetAllSocialPostsQuery request, CancellationToken cancellationToken)
    {
        var socialPosts = await _socialPostRepository.GetAllAsync(cancellationToken);
        return socialPosts.ToList();
    }
}
