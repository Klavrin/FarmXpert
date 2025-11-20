using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteSocialPost;

public class DeleteSocialPostCommandHandler : IRequestHandler<DeleteSocialPostCommand, FarmXpert.Domain.Entities.SocialPost?>
{
    private readonly ISocialPostRepository _socialPostRepository;
    private readonly IFileStorageService _fileStorageServiceRepository;
    public DeleteSocialPostCommandHandler(ISocialPostRepository socialPostRepository, IFileStorageService fileStorageServiceRepository)
    {
        _socialPostRepository = socialPostRepository;
        _fileStorageServiceRepository = fileStorageServiceRepository;
    }
    public async Task<FarmXpert.Domain.Entities.SocialPost?> Handle(DeleteSocialPostCommand request, CancellationToken cancellationToken)
    {
        var socialPost = await _socialPostRepository.GetByIdAsync(request.Id, cancellationToken);
        if (socialPost == null)
        {
            return null;
        }
        var fileUrl = socialPost.Url;
        if (fileUrl != null)
        {
            await _fileStorageServiceRepository.DeleteFileAsync(fileUrl);
        }
        await _socialPostRepository.DeleteAsync(request.Id, cancellationToken);
        return socialPost;
    }
}
