using MediatR;

namespace FarmXpert.Application.PersonalDocument.Queries.GetPersonalDocumentById;

public class GetPersonalDocumentByIdQueryHandler : IRequestHandler<GetPersonalDocumentByIdQuery, FarmXpert.Domain.Entities.PersonalDocument>
{
    private readonly Domain.Interfaces.IPersonalDocumentRepository _personalDocumentRepository;
    public GetPersonalDocumentByIdQueryHandler(Domain.Interfaces.IPersonalDocumentRepository personalDocumentRepository)
    {
        _personalDocumentRepository = personalDocumentRepository;
    }
    public async Task<FarmXpert.Domain.Entities.PersonalDocument> Handle(GetPersonalDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var personalDocument = await _personalDocumentRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        return personalDocument;
    }
}
