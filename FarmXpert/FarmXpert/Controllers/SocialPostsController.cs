using FarmXpert.Application.SocialPost.Commands.AddComment;
using FarmXpert.Application.SocialPost.Commands.AddLike;
using FarmXpert.Application.SocialPost.Commands.CreateSocialPost;
using FarmXpert.Application.SocialPost.Commands.DeleteComment;
using FarmXpert.Application.SocialPost.Commands.DeleteLike;
using FarmXpert.Application.SocialPost.Commands.DeleteSocialPost;
using FarmXpert.Application.SocialPost.Commands.EditSocialPost;
using FarmXpert.Application.SocialPost.Queries.GetAllSocialPosts;
using FarmXpert.Application.SocialPost.Queries.GetSocialPostById;
using FarmXpert.Application.SocialPost.Queries.GetUserSocialPost;
using FarmXpert.Data;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/socialposts")]
[Authorize]
public class SocialPostsController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly UserManager<ApplicationUser> _userManager;

    public SocialPostsController(IMediator mediator, UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    /// <summary>
    /// Retrieves all social posts from all users.
    /// </summary>
    /// <returns>A list of all social posts.</returns>
    /// <response code="200">Returns the list of social posts.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<SocialPost> socialPosts = await _mediator.Send(new GetAllSocialPostsQuery());
        return Ok(socialPosts);
    }

    /// <summary>
    /// Retrieves a specific social post by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the social post.</param>
    /// <returns>The social post details if found.</returns>
    /// <response code="200">Returns the social post details.</response>
    /// <response code="404">If the social post is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var socialPost = await _mediator.Send(new GetSocialPostByIdQuery(id));
        if (socialPost == null)
        {
            return NotFound();
        }
        return Ok(socialPost);
    }

    /// <summary>
    /// Retrieves all social posts for a specific business.
    /// </summary>
    /// <param name="BusinessId">The business identifier.</param>
    /// <returns>A list of social posts for the specified business.</returns>
    /// <response code="200">Returns the list of social posts for the business.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("user/{BusinessId}")]
    public async Task<IActionResult> GetUserSocialPosts(string BusinessId)
    {
        var socialPosts = await _mediator.Send(new GetUserSocialPostQuery(BusinessId));
        return Ok(socialPosts);
    }

    /// <summary>
    /// Creates a new social post with an image or video attachment.
    /// </summary>
    /// <param name="request">The request containing the post title, content, and media file.</param>
    /// <returns>The newly created social post.</returns>
    /// <response code="201">Returns the newly created social post.</response>
    /// <response code="400">If the file type is invalid.</response>
    /// <response code="401">If the user is not authenticated or not found.</response>
    [HttpPost]
    public async Task<IActionResult> Create(CreateSocialPostRequest request)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized("User not found");
        var businessId = user.BusinessId;
        string extension = Path.GetExtension(request.File.FileName);

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" };

        if (!allowedExtensions.Contains(extension))
        {
            return BadRequest("Invalid file type");
        }

        using var stream = request.File.OpenReadStream();
        var command = new CreateSocialPostCommand(request.Title, request.Content, stream, extension, businessId);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Deletes a specific social post by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the social post to delete.</param>
    /// <returns>The deleted social post details.</returns>
    /// <response code="200">Returns the deleted social post details.</response>
    /// <response code="404">If the social post is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteSocialPostCommand(id);
        var result = await _mediator.Send(command);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing social post's title and content.
    /// </summary>
    /// <param name="id">The unique identifier of the social post to update.</param>
    /// <param name="Title">The updated title for the post.</param>
    /// <param name="Content">The updated content for the post.</param>
    /// <returns>The updated social post details.</returns>
    /// <response code="200">Returns the updated social post.</response>
    /// <response code="404">If the social post is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, string Title, string Content)
    {
        var result = await _mediator.Send(new EditSocialPostCommand(id, Title, Content));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Adds a like to a specific social post.
    /// </summary>
    /// <param name="id">The unique identifier of the social post to like.</param>
    /// <returns>The updated social post with the new like.</returns>
    /// <response code="200">Returns the updated social post.</response>
    /// <response code="404">If the social post is not found.</response>
    /// <response code="401">If the user is not authenticated or not found.</response>
    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> AddLike(Guid id)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized("User not found");
        var businessId = user.BusinessId;
        var result = await _mediator.Send(new AddLikeCommand(id, businessId));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Removes a like from a specific social post.
    /// </summary>
    /// <param name="id">The unique identifier of the social post to unlike.</param>
    /// <returns>The updated social post without the like.</returns>
    /// <response code="200">Returns the updated social post.</response>
    /// <response code="404">If the social post is not found.</response>
    /// <response code="401">If the user is not authenticated or not found.</response>
    [HttpDelete("{id:guid}/like")]
    public async Task<IActionResult> DeleteLike(Guid id)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized("User not found");
        var businessId = user.BusinessId;
        var result = await _mediator.Send(new DeleteLikeCommand(id, businessId));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Adds a comment to a specific social post.
    /// </summary>
    /// <param name="id">The unique identifier of the social post to comment on.</param>
    /// <param name="Content">The content of the comment.</param>
    /// <returns>The updated social post with the new comment.</returns>
    /// <response code="200">Returns the updated social post.</response>
    /// <response code="404">If the social post is not found.</response>
    /// <response code="401">If the user is not authenticated or not found.</response>
    [HttpPost("{id:guid}/comment")]
    public async Task<IActionResult> AddComment(Guid id, string Content)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized("User not found");
        var businessId = user.BusinessId;

        var comment = new Comment
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Content = Content,
            businessId = businessId
        };
        var result = await _mediator.Send(new AddCommentCommand(id, comment));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    /// <summary>
    /// Deletes a comment from a specific social post.
    /// </summary>
    /// <param name="id">The unique identifier of the social post.</param>
    /// <param name="commentId">The unique identifier of the comment to delete.</param>
    /// <returns>The updated social post without the deleted comment.</returns>
    /// <response code="200">Returns the updated social post.</response>
    /// <response code="404">If the social post or comment is not found.</response>
    /// <response code="401">If the user is not authenticated or not found.</response>
    [HttpDelete("{id:guid}/comment")]
    public async Task<IActionResult> DeleteComment(Guid id, Guid commentId)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Unauthorized("User not found");
        var businessId = user.BusinessId;

        var result = await _mediator.Send(new DeleteCommentCommand(id, commentId, businessId));
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    public class CreateSocialPostRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
    }
}
