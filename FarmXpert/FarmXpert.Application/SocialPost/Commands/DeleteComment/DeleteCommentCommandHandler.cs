using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, FarmXpert.Domain.Entities.SocialPost>
{
    private readonly Domain.Interfaces.ISocialPostRepository _socialPostRepository;
    public DeleteCommentCommandHandler(Domain.Interfaces.ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }

    public async Task<FarmXpert.Domain.Entities.SocialPost> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var socialPost = await _socialPostRepository.GetByIdAsync(request.Id, cancellationToken);
        if (socialPost == null)
        {
            return null;
        }
        if (request.BusinessId != socialPost.BusinessId)
        {
            return null;
        }
        var comment = socialPost.Comments.FirstOrDefault(c => c.Id == request.Id);
        if (comment == null)
        {
            return null;
        }
        socialPost.Comments.Remove(comment);
        socialPost.CommentsCount--;
        await _socialPostRepository.UpdateAsync(socialPost, cancellationToken);
        return socialPost;
    }
}
