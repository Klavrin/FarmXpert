using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteComment;

public record DeleteCommentCommand(Guid Id, string BusinessId) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
