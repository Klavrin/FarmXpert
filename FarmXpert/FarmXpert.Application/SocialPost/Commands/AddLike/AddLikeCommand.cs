using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.AddLike;
public record AddLikeCommand(Guid id, string BusinessId) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
