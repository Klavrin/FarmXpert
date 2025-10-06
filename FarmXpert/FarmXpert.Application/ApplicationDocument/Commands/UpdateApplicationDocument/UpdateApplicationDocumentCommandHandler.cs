using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Commands.UpdateApplicationDocument;

public class UpdateApplicationDocumentCommandHandler : IRequestHandler<UpdateApplicationDocumentCommand, FarmXpert.Domain.Entities.ApplicationDocument?>
{
    private readonly Domain.Interfaces.IApplicationDocumentRepository _applicationDocumentRepository;
    public UpdateApplicationDocumentCommandHandler(Domain.Interfaces.IApplicationDocumentRepository applicationDocumentRepository)
    {
        _applicationDocumentRepository = applicationDocumentRepository;
    }
    public async Task<FarmXpert.Domain.Entities.ApplicationDocument?> Handle(UpdateApplicationDocumentCommand request, CancellationToken cancellationToken)
    {
        var applicationDocument = await _applicationDocumentRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        if (applicationDocument == null)
        {
            return null;
        }
        applicationDocument.Title = request.Title;
        applicationDocument.Status = request.Status;
        applicationDocument.RejectionReason = request.ReasonRejection;
        await _applicationDocumentRepository.UpdateAsync(applicationDocument, cancellationToken);
        return applicationDocument;
    }
}
