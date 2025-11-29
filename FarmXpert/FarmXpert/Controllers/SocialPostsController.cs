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
public class SocialPostsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public SocialPostsController(IMediator mediator, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<SocialPost> socialPosts = await _mediator.Send(new GetAllSocialPostsQuery());
        return Ok(socialPosts);
    }

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

    [HttpGet("user/{BusinessId}")]
    public async Task<IActionResult> GetUserSocialPosts(string BusinessId)
    {
        var socialPosts = await _mediator.Send(new GetUserSocialPostQuery(BusinessId));
        return Ok(socialPosts);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSocialPostRequest request)
    {
        var userId = CurrentUserId();
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

    [HttpPost("{id:guid}/like")]
    public async Task<IActionResult> AddLike(Guid id)
    {
        var userId = CurrentUserId();
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

    [HttpDelete("{id:guid}/like")]
    public async Task<IActionResult> DeleteLike(Guid id)
    {
        var userId = CurrentUserId();
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

    [HttpPost("{id:guid}/comment")]
    public async Task<IActionResult> AddComment(Guid id, string Content)
    {
        var userId = CurrentUserId();
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

    [HttpDelete("{id:guid}/comment")]
    public async Task<IActionResult> DeleteComment(Guid id, Guid commentId)
    {
        var userId = CurrentUserId();
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

    private string CurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated");
    }
}
