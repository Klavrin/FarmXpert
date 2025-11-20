using FarmXpert.Domain.Entities;
using MediatR;

namespace FarmXpert.Application.SocialPost.Commands.AddComment;

public record AddCommentCommand(Guid id, Comment comment) : IRequest<FarmXpert.Domain.Entities.SocialPost>;
