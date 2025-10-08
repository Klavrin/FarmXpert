using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.DeleteComment;

public record DeleteCommentCommand(Guid PostId,Guid CommentId, string BusinessId) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
