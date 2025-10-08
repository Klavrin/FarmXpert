using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.EditSocialPost;
public record EditSocialPostCommand(Guid Id, string Title, string Content) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
