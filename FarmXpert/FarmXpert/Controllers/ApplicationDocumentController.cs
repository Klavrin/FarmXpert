using FarmXpert.Application.ApplicationDocument.Commands.CreateApplicationDocument;
using FarmXpert.Application.ApplicationDocument.Commands.DeleteApplicationDocument;
using FarmXpert.Application.ApplicationDocument.Commands.UpdateApplicationDocument;
using FarmXpert.Application.ApplicationDocument.Queries.GetAllApplicationDocuments;
using FarmXpert.Application.ApplicationDocument.Queries.GetApplicationDocumentById;
using FarmXpert.Application.PersonalDocument.Commands.DeletePersonalDocument;
using FarmXpert.Application.PersonalDocument.Queries.GetPersonalDocumentById;
using FarmXpert.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/applicationDocuments")]
[Authorize]

public class ApplicationDocumentController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IMediator _mediator;

    public ApplicationDocumentController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = CurrentUserId();
        var documents = await _mediator.Send(new GetAllApplicationDocumentsQuery(userId));
        return Ok(documents);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = CurrentUserId();
        var document = await _mediator.Send(new GetApplicationDocumentByIdQuery(id, userId));
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
        var applicationDocument = await _mediator.Send(new GetApplicationDocumentByIdQuery(id, userId));
        if (applicationDocument == null)
            return NotFound();

        if (!System.IO.File.Exists(applicationDocument.Url))
            return NotFound("File not found on disk.");

        var stream = new FileStream(applicationDocument.Url, FileMode.Open, FileAccess.Read);
        return File(stream, "application/octet-stream", applicationDocument.Title + applicationDocument.FileExtension);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, string Title, string Status, string RejectionReason)
    {
        var userId = CurrentUserId();
        var command = new UpdateApplicationDocumentCommand(id, userId, Title, Status, RejectionReason);
        var updatedApplicationDocument = await _mediator.Send(command);
        if (updatedApplicationDocument == null)
            return NotFound();
        return Ok(updatedApplicationDocument);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateApplicationDocumentRequest request)
    {
        var userId = CurrentUserId();
        var Title = request.Title;
        var File = request.File;

        if (File == null || File.Length == 0)
            return BadRequest("File is required.");

        string originalFileName = request.File.FileName;
        string extension = Path.GetExtension(request.File.FileName);

        using var stream = File.OpenReadStream();
        var command = new CreateApplicationDocumentCommand(Title, stream, extension, userId);

        var created = await _mediator.Send(command);

        if (created == null)
            return BadRequest("Invalid file type. Only PDF files are allowed.");

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = CurrentUserId();
        var applicationDocument = await _mediator.Send(new GetApplicationDocumentByIdQuery(id, userId));
        if (applicationDocument == null)
            return NotFound();
        var command = new DeleteApplicationDocumentCommand(id, userId);
        await _mediator.Send(command);
        return Ok(applicationDocument);
    }

    public class CreateApplicationDocumentRequest
    {
        public string Title { get; set; }
        public IFormFile File { get; set; }
    }
    private string CurrentUserId()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("User not authenticated");
    }
}
