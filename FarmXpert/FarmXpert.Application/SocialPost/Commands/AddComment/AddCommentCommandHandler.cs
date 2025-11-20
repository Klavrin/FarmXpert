using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.AddComment;
public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, FarmXpert.Domain.Entities.SocialPost>
{
    private readonly Domain.Interfaces.ISocialPostRepository _socialPostRepository;
    public AddCommentCommandHandler(Domain.Interfaces.ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }
    public async Task<FarmXpert.Domain.Entities.SocialPost> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var socialPost = await _socialPostRepository.GetByIdAsync(request.id, cancellationToken);
        if (socialPost == null)
        {
            return null;
        }
        if (request.comment == null)
        {
            return null;
        }
        request.comment.Id = Guid.NewGuid();
        socialPost.Comments.Add(request.comment);
        socialPost.CommentsCount++;
        await _socialPostRepository.UpdateAsync(socialPost, cancellationToken);

        return socialPost;
    }
}
