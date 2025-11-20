using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteLike;
public record DeleteLikeCommand(Guid id, string BusinessId) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
