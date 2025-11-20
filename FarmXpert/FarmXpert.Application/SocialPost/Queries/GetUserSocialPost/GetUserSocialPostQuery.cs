using MediatR;

namespace FarmXpert.Application.SocialPost.Queries.GetUserSocialPost;

public record GetUserSocialPostQuery(string BusinessId) : IRequest<List<FarmXpert.Domain.Entities.SocialPost>>;
