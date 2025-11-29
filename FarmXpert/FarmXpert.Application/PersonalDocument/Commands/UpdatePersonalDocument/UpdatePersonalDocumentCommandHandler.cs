using MediatR;

namespace FarmXpert.Application.PersonalDocument.Commands.UpdatePersonalDocument;

public class UpdatePersonalDocumentCommandHandler : IRequestHandler<UpdatePersonalDocumentCommand, FarmXpert.Domain.Entities.PersonalDocument?>
{
    private readonly Domain.Interfaces.IPersonalDocumentRepository _personalDocumentRepository;
    public UpdatePersonalDocumentCommandHandler(Domain.Interfaces.IPersonalDocumentRepository personalDocumentRepository)
    {
        _personalDocumentRepository = personalDocumentRepository;
    }

    public async Task<FarmXpert.Domain.Entities.PersonalDocument?> Handle(UpdatePersonalDocumentCommand request, CancellationToken cancellationToken)
    {
        var existingPersonalDocument = await _personalDocumentRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        if (existingPersonalDocument == null)
        {
            return null;
        }
        existingPersonalDocument.Title = request.Title;
        await _personalDocumentRepository.UpdateAsync(existingPersonalDocument, cancellationToken);
        return existingPersonalDocument;
    }
}
