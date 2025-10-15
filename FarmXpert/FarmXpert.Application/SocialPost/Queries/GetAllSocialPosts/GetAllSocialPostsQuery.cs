using MediatR;

namespace FarmXpert.Application.SocialPost.Queries.GetAllSocialPosts;

public record GetAllSocialPostsQuery: IRequest<List<FarmXpert.Domain.Entities.SocialPost>>;
