using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.CreateSocialPost;

public record CreateSocialPostCommand(string Title, string Content, Stream  FileStream, string FileExtension, string BusinessId) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
