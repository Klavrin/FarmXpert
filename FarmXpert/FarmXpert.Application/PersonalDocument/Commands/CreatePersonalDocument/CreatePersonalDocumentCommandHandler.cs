using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.PersonalDocument.Commands.CreatePersonalDocument;

public class CreatePersonalDocumentCommandHandler : IRequestHandler<CreatePersonalDocumentCommand, FarmXpert.Domain.Entities.PersonalDocument>
{
    private readonly IPersonalDocumentRepository _personalDocumentRepository;
    private readonly IFileStorageService _fileStorageService;


    public CreatePersonalDocumentCommandHandler(IPersonalDocumentRepository personalDocumentRepository, IFileStorageService fileStorageService)
    {
        _personalDocumentRepository = personalDocumentRepository;
        _fileStorageService = fileStorageService;
    }

    public async Task<FarmXpert.Domain.Entities.PersonalDocument> Handle(CreatePersonalDocumentCommand request, CancellationToken cancellationToken)
    {
        var NewId = Guid.NewGuid();
        var fileUrl = await _fileStorageService.SaveFileAsync(request.FileStream, request.OwnerId + "."+ NewId.ToString());

        var personaldocument = new FarmXpert.Domain.Entities.PersonalDocument
        {
            Id = NewId,
            Title = request.Title,
            UploadDate = DateTime.UtcNow, 
            Url = fileUrl,
            FileExtension = request.FileExtension,
            OwnerId = request.OwnerId
        };
        await _personalDocumentRepository.CreateAsync(personaldocument, cancellationToken);
        return personaldocument;
    }
}