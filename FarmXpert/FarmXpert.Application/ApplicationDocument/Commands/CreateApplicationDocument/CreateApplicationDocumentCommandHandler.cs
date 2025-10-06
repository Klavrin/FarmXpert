using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Commands.CreateApplicationDocument;

public class CreateApplicationDocumentCommandHandler : IRequestHandler<CreateApplicationDocumentCommand, FarmXpert.Domain.Entities.ApplicationDocument>
{
    private readonly IApplicationDocumentRepository _applicationDocumentRepository;
    private readonly IFileStorageService _fileStorageService;

    public CreateApplicationDocumentCommandHandler(IApplicationDocumentRepository applicationDocumentRepository, IFileStorageService fileStorageService)
    {
        _applicationDocumentRepository = applicationDocumentRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<FarmXpert.Domain.Entities.ApplicationDocument> Handle(CreateApplicationDocumentCommand request, CancellationToken cancellationToken)
    {
        var NewId = Guid.NewGuid();
        var fileUrl = await _fileStorageService.SaveFileAsync(request.FileStream, request.OwnerId + "." + NewId.ToString() + ".application");
        var applicationDocument = new FarmXpert.Domain.Entities.ApplicationDocument
        {
            Id = NewId,
            Title = request.Title,
            UploadDate = DateTime.UtcNow,
            Url = fileUrl,
            FileExtension = request.FileExtension,
            OwnerId = request.OwnerId,
            Status = "Pending",
            RejectionReason = string.Empty
        };
        await _applicationDocumentRepository.CreateAsync(applicationDocument, cancellationToken);
        return applicationDocument;
    }
}
