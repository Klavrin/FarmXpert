using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteLike;
public class DeleteLikeCommandHandler : IRequestHandler<DeleteLikeCommand, FarmXpert.Domain.Entities.SocialPost>
{
    private readonly Domain.Interfaces.ISocialPostRepository _socialPostRepository;
    public DeleteLikeCommandHandler(Domain.Interfaces.ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }
    public async Task<FarmXpert.Domain.Entities.SocialPost> Handle(DeleteLikeCommand request, CancellationToken cancellationToken)
    {
        var socialPost = await _socialPostRepository.GetByIdAsync(request.id, cancellationToken);
        if (socialPost == null)
        {
            return null;
        }
        if (!socialPost.Likes.Contains(request.BusinessId))
        {
            return null;
        }
        socialPost.Likes.Remove(request.BusinessId);
        socialPost.LikesCount--;
        await _socialPostRepository.UpdateAsync(socialPost, cancellationToken);
        return socialPost;
    }
}
