using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.EditSocialPost;
public class EditSocialPostCommandHandler : IRequestHandler<EditSocialPostCommand, FarmXpert.Domain.Entities.SocialPost>
{
    private readonly Domain.Interfaces.ISocialPostRepository _socialPostRepository;
    public EditSocialPostCommandHandler(Domain.Interfaces.ISocialPostRepository socialPostRepository)
    {
        _socialPostRepository = socialPostRepository;
    }
    public async Task<FarmXpert.Domain.Entities.SocialPost> Handle(EditSocialPostCommand request, CancellationToken cancellationToken)
    {
        var socialPost = await _socialPostRepository.GetByIdAsync(request.Id, cancellationToken);
        if (socialPost == null)
        {
            throw new KeyNotFoundException($"Social post with ID {request.Id} not found.");
        }
        socialPost.Title = request.Title;
        socialPost.Content = request.Content;
        await _socialPostRepository.UpdateAsync(socialPost, cancellationToken);
        return socialPost;
    }
}
