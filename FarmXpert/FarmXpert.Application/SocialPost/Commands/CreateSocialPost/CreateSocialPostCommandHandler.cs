using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.CreateSocialPost;

public class CreateSocialPostCommandHandler : IRequestHandler<CreateSocialPostCommand, FarmXpert.Domain.Entities.SocialPost>
{
    private readonly Domain.Interfaces.ISocialPostRepository _socialPostRepository;
    private readonly Domain.Interfaces.IFileStorageService _fileStorageService;
    public CreateSocialPostCommandHandler(Domain.Interfaces.ISocialPostRepository socialPostRepository, Domain.Interfaces.IFileStorageService fileStorageService)
    {
        _socialPostRepository = socialPostRepository;
        _fileStorageService = fileStorageService;
    }
    public async Task<FarmXpert.Domain.Entities.SocialPost> Handle(CreateSocialPostCommand request, CancellationToken cancellationToken)
    {
        var NewId = Guid.NewGuid();
        var fileUrl = await _fileStorageService.SaveFileAsync(request.FileStream, request.BusinessId + "." + NewId.ToString() + ".postFile");
        var socialPost = new FarmXpert.Domain.Entities.SocialPost
        {
            Id = NewId,
            Title = request.Title,
            Content = request.Content,
            Url = fileUrl,
            BusinessId = request.BusinessId,
            UploadDate = DateTime.UtcNow,
            Comments = new List<FarmXpert.Domain.Entities.Comment>(),
            Likes = new List<string>(),
            LikesCount = 0,
            CommentsCount = 0
        };
        await _socialPostRepository.CreateAsync(socialPost, cancellationToken);
        return socialPost;
    }
}
