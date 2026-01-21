using FarmXpert.Application.PersonalDocument.Commands.CreatePersonalDocument;
using FarmXpert.Application.PersonalDocument.Commands.DeletePersonalDocument;
using FarmXpert.Application.PersonalDocument.Commands.UpdatePersonalDocument;
using FarmXpert.Application.PersonalDocument.Queries.GetAllPersonalDocuments;
using FarmXpert.Application.PersonalDocument.Queries.GetPersonalDocumentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FarmXpert.Controllers;

[ApiController]
[Route("api/personalDocuments")]
[Authorize]
public class PersonalDocumentController : BaseApiController
{
    private readonly IMediator _mediator;

    public PersonalDocumentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all personal documents for the current user.
    /// </summary>
    /// <returns>A list of all personal documents owned by the authenticated user.</returns>
    /// <response code="200">Returns the list of personal documents.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var documents = await _mediator.Send(new GetAllPersonalDocumentsQuery(userId));
        return Ok(documents);
    }

    /// <summary>
    /// Retrieves a specific personal document by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the personal document.</param>
    /// <returns>The personal document details if found.</returns>
    /// <response code="200">Returns the personal document details.</response>
    /// <response code="404">If the personal document is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetCurrentUserId();
        var document = await _mediator.Send(new GetPersonalDocumentByIdQuery(id, userId));
        if (document == null)
        {
            return NotFound();
        }
        return Ok(document);
    }

    /// <summary>
    /// Downloads a specific personal document file by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the personal document to download.</param>
    /// <returns>The file stream for download.</returns>
    /// <response code="200">Returns the file for download.</response>
    /// <response code="404">If the personal document or file is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var userId = GetCurrentUserId();
        var document = await _mediator.Send(new GetPersonalDocumentByIdQuery(id, userId));
        if (document == null)
            return NotFound();
        if (!System.IO.File.Exists(document.Url))
            return NotFound("File not found on disk.");
        var stream = new FileStream(document.Url, FileMode.Open, FileAccess.Read);
        return File(stream, "application/octet-stream", document.Title + document.FileExtension);
    }

    /// <summary>
    /// Updates an existing personal document's title.
    /// </summary>
    /// <param name="id">The unique identifier of the personal document to update.</param>
    /// <param name="Title">The updated title for the document.</param>
    /// <returns>The updated personal document details.</returns>
    /// <response code="200">Returns the updated personal document.</response>
    /// <response code="404">If the personal document is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, string Title)
    {
        var userId = GetCurrentUserId();
        var command = new UpdatePersonalDocumentCommand(id, userId, Title);
        var updatedDocument = await _mediator.Send(command);
        if (updatedDocument == null)
            return NotFound();
        return Ok(updatedDocument);
    }

    /// <summary>
    /// Creates a new personal document by uploading a file.
    /// </summary>
    /// <param name="request">The request containing the document title and file to upload.</param>
    /// <returns>The newly created personal document.</returns>
    /// <response code="201">Returns the newly created personal document.</response>
    /// <response code="400">If the file is missing or empty.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonalDocumentRequest request)
    {
        var Title = request.Title;
        var File = request.File;
        var userId = GetCurrentUserId();
        if (File == null || File.Length == 0)
            return BadRequest("File is required.");
        string originalFileName = request.File.FileName;
        string extension = Path.GetExtension(request.File.FileName);
        using var stream = File.OpenReadStream();
        var command = new CreatePersonalDocumentCommand(Title, stream, extension, userId);
        var created = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Deletes a specific personal document by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the personal document to delete.</param>
    /// <returns>The deleted personal document details.</returns>
    /// <response code="200">Returns the deleted personal document details.</response>
    /// <response code="404">If the personal document is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
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
}
