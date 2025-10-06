using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Commands.DeleteApplicationDocument;

public class DeleteApplicationDocumentCommandHandler : IRequestHandler<DeleteApplicationDocumentCommand, FarmXpert.Domain.Entities.ApplicationDocument?>
{
    private readonly IApplicationDocumentRepository _applicationDocumentRepository;
    private readonly IFileStorageService _fileStorageServiceRepository;

    public DeleteApplicationDocumentCommandHandler(IApplicationDocumentRepository applicationDocumentRepository, IFileStorageService fileStorageServiceRepository)
    {
        _applicationDocumentRepository = applicationDocumentRepository;
        _fileStorageServiceRepository = fileStorageServiceRepository;
    }

    public async Task<FarmXpert.Domain.Entities.ApplicationDocument?> Handle(DeleteApplicationDocumentCommand request, CancellationToken cancellationToken)
    {
        var applicationDocument = await _applicationDocumentRepository.GetByIdAsync(request.OwnerId, request.id, cancellationToken);
        if (applicationDocument == null)
        {
            return null;
        }
        var fileUrl = applicationDocument.Url;
        if(fileUrl != null)
        {
            await _fileStorageServiceRepository.DeleteFileAsync(fileUrl);
        }
        await _applicationDocumentRepository.DeleteAsync(request.OwnerId, request.id, cancellationToken);
        return applicationDocument;
    }
}
