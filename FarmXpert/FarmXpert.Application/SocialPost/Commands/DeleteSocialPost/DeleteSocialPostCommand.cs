using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteSocialPost;

public record DeleteSocialPostCommand (Guid Id) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
