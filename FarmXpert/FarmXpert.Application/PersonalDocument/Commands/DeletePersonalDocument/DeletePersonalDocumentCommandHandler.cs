using MediatR;

namespace FarmXpert.Application.PersonalDocument.Commands.DeletePersonalDocument;

public class DeletePersonalDocumentCommandHandler : IRequestHandler<DeletePersonalDocumentCommand, FarmXpert.Domain.Entities.PersonalDocument?>
{
    private readonly Domain.Interfaces.IPersonalDocumentRepository _personalDocumentRepository;
    private readonly Domain.Interfaces.IFileStorageService _fileStorageServiceRepository;   
    public DeletePersonalDocumentCommandHandler(Domain.Interfaces.IPersonalDocumentRepository personalDocumentRepository, Domain.Interfaces.IFileStorageService fileStorageServiceRepository)
    {
        _personalDocumentRepository = personalDocumentRepository;
        _fileStorageServiceRepository = fileStorageServiceRepository;
    }
    public async Task<FarmXpert.Domain.Entities.PersonalDocument?> Handle(DeletePersonalDocumentCommand request, CancellationToken cancellationToken)
    {
        var personaldocument = await _personalDocumentRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        if (personaldocument == null)
        {
            return null;
        }

        var fileUrl = personaldocument.Url;

        if(fileUrl != null)
        {
            await _fileStorageServiceRepository.DeleteFileAsync(fileUrl);
        }

        await _personalDocumentRepository.DeleteAsync(request.OwnerId, request.Id, cancellationToken);
        return personaldocument;
    }
}