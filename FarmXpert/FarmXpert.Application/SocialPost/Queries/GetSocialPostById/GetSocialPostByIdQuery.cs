using MediatR;

namespace FarmXpert.Application.SocialPost.Queries.GetSocialPostById;

public record GetSocialPostByIdQuery(Guid id) : IRequest<Domain.Entities.SocialPost>;