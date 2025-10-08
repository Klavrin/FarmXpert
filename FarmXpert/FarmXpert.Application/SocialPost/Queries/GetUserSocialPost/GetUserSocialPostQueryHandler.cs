using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.SocialPost.Queries.GetUserSocialPost;

public class GetUserSocialPostQueryHandler : IRequestHandler<GetUserSocialPostQuery, List<FarmXpert.Domain.Entities.SocialPost>>
{
    private readonly ISocialPostRepository _socialPostRepository;
    public GetUserSocialPostQueryHandler(ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }
    public async Task<List<FarmXpert.Domain.Entities.SocialPost>> Handle(GetUserSocialPostQuery request, CancellationToken cancellationToken)
    {
        var posts = await _socialPostRepository.GetAllByUserAsync(request.BusinessId, cancellationToken);
        return posts.ToList();
    }
}
