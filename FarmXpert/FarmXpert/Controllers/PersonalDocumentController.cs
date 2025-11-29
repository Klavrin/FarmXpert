using FarmXpert.Application.PersonalDocument.Commands.CreatePersonalDocument;
using FarmXpert.Application.PersonalDocument.Commands.DeletePersonalDocument;
using FarmXpert.Application.PersonalDocument.Commands.UpdatePersonalDocument;
using FarmXpert.Application.PersonalDocument.Queries.GetAllPersonalDocuments;
using FarmXpert.Application.PersonalDocument.Queries.GetPersonalDocumentById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/personalDocuments")]
[Authorize]

public class PersonalDocumentController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMediator _mediator;

    public PersonalDocumentController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = CurrentUserId();
        var documents = await _mediator.Send(new GetAllPersonalDocumentsQuery(userId));
        return Ok(documents);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = CurrentUserId();
        var document = await _mediator.Send(new GetPersonalDocumentByIdQuery(id, userId));
        if (document == null)
        {
            return NotFound();
        }
        return Ok(document);
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var userId = CurrentUserId();
        var document = await _mediator.Send(new GetPersonalDocumentByIdQuery(id, userId));
        if (document == null)
            return NotFound();

        if (!System.IO.File.Exists(document.Url))
            return NotFound("File not found on disk.");

        var stream = new FileStream(document.Url, FileMode.Open, FileAccess.Read);
        return File(stream, "application/octet-stream", document.Title + document.FileExtension);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, string Title)
    {
        var userId = CurrentUserId();
        var command = new UpdatePersonalDocumentCommand(id, userId, Title);
        var updatedDocument = await _mediator.Send(command);
        if (updatedDocument == null)
            return NotFound();
        return Ok(updatedDocument);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonalDocumentRequest request)
    {
        var Title = request.Title;
        var File = request.File;
        var userId = CurrentUserId();

        if (File == null || File.Length == 0)
            return BadRequest("File is required.");

        string originalFileName = request.File.FileName;
        string extension = Path.GetExtension(request.File.FileName);

        using var stream = File.OpenReadStream();
        var command = new CreatePersonalDocumentCommand(Title, stream, extension, userId);

        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = CurrentUserId();
        var personalDocument = await _mediator.Send(new GetPersonalDocumentByIdQuery(id, userId));
        if (personalDocument == null)
            return NotFound();
        var command = new DeletePersonalDocumentCommand(id, userId);
        await _mediator.Send(command);
        return Ok(personalDocument);
    }

    public class CreatePersonalDocumentRequest
    {
        public string Title { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }

    private string CurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated");
    }
}
