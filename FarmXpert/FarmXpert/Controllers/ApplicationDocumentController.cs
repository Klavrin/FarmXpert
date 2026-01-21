using FarmXpert.Application.ApplicationDocument.Commands.CreateApplicationDocument;
using FarmXpert.Application.ApplicationDocument.Commands.DeleteApplicationDocument;
using FarmXpert.Application.ApplicationDocument.Commands.UpdateApplicationDocument;
using FarmXpert.Application.ApplicationDocument.Queries.GetAllApplicationDocuments;
using FarmXpert.Application.ApplicationDocument.Queries.GetApplicationDocumentById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmXpert.Controllers;

[ApiController]
[Route("api/applicationDocuments")]
[Authorize]

public class ApplicationDocumentController : BaseApiController
{
    private readonly IMediator _mediator;

    public ApplicationDocumentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all application documents for the current user.
    /// </summary>
    /// <returns>A list of all application documents owned by the authenticated user.</returns>
    /// <response code="200">Returns the list of application documents.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var documents = await _mediator.Send(new GetAllApplicationDocumentsQuery(userId));
        return Ok(documents);
    }

    /// <summary>
    /// Retrieves a specific application document by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the application document.</param>
    /// <returns>The application document details if found.</returns>
    /// <response code="200">Returns the application document details.</response>
    /// <response code="404">If the application document is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = GetCurrentUserId();
        var document = await _mediator.Send(new GetApplicationDocumentByIdQuery(id, userId));
        if (document == null)
        {
            return NotFound();
        }
        return Ok(document);
    }

    /// <summary>
    /// Downloads a specific application document file by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the application document to download.</param>
    /// <returns>The file stream for download.</returns>
    /// <response code="200">Returns the file for download.</response>
    /// <response code="404">If the application document or file is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id)
    {
        var userId = GetCurrentUserId();
        var applicationDocument = await _mediator.Send(new GetApplicationDocumentByIdQuery(id, userId));
        if (applicationDocument == null)
            return NotFound();

        if (!System.IO.File.Exists(applicationDocument.Url))
            return NotFound("File not found on disk.");

        var stream = new FileStream(applicationDocument.Url, FileMode.Open, FileAccess.Read);
        return File(stream, "application/octet-stream", applicationDocument.Title + applicationDocument.FileExtension);
    }

    /// <summary>
    /// Updates an existing application document's information.
    /// </summary>
    /// <param name="id">The unique identifier of the application document to update.</param>
    /// <param name="Title">The updated title for the document.</param>
    /// <param name="Status">The updated status for the document.</param>
    /// <param name="RejectionReason">The rejection reason if applicable.</param>
    /// <returns>The updated application document details.</returns>
    /// <response code="200">Returns the updated application document.</response>
    /// <response code="404">If the application document is not found.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, string Title, string Status, string RejectionReason)
    {
        var userId = GetCurrentUserId();
        var command = new UpdateApplicationDocumentCommand(id, userId, Title, Status, RejectionReason);
        var updatedApplicationDocument = await _mediator.Send(command);
        if (updatedApplicationDocument == null)
            return NotFound();
        return Ok(updatedApplicationDocument);
    }

    /// <summary>
    /// Creates a new application document by uploading a PDF file.
    /// </summary>
    /// <param name="request">The request containing the document title and file to upload.</param>
    /// <returns>The newly created application document.</returns>
    /// <response code="201">Returns the newly created application document.</response>
    /// <response code="400">If the file is missing, empty, or not a PDF.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    public async Task<IActionResult> Create(CreateApplicationDocumentRequest request)
    {
        var userId = GetCurrentUserId();
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

    /// <summary>
    /// Deletes a specific application document by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the application document to delete.</param>
    /// <returns>The deleted application document details.</returns>
    /// <response code="200">Returns the deleted application document details.</response>
    /// <response code="404">If the application document is not found or does not belong to the user.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
        var applicationDocument = await _mediator.Send(new GetApplicationDocumentByIdQuery(id, userId));
        if (applicationDocument == null)
            return NotFound();
        var command = new DeleteApplicationDocumentCommand(id, userId);
        await _mediator.Send(command);
        return Ok(applicationDocument);
    }

    /// <summary>
    /// Request model for creating an application document.
    /// </summary>
    public class CreateApplicationDocumentRequest
    {
        /// <summary>
        /// The title of the application document.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The PDF file to upload.
        /// </summary>
        public IFormFile File { get; set; } = null!;
    }
}
