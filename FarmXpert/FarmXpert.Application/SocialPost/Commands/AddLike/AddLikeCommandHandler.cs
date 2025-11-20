using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.AddLike;
public class AddLikeCommandHandler : IRequestHandler<AddLikeCommand, FarmXpert.Domain.Entities.SocialPost>
{
    private readonly Domain.Interfaces.ISocialPostRepository _socialPostRepository;
    public AddLikeCommandHandler(Domain.Interfaces.ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }
    public async Task<FarmXpert.Domain.Entities.SocialPost> Handle(AddLikeCommand request, CancellationToken cancellationToken)
    {
        var socialPost = await _socialPostRepository.GetByIdAsync(request.id, cancellationToken);
        if (socialPost == null)
        {
            return null;
        }
        if (socialPost.Likes.Contains(request.BusinessId))
        {
            return null;
        }
        socialPost.Likes.Add(request.BusinessId);
        socialPost.LikesCount++;
        await _socialPostRepository.UpdateAsync(socialPost, cancellationToken);
        return socialPost;
    }
}
