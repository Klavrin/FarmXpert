using MediatR;

namespace FarmXpert.Application.PersonalDocument.Queries.GetAllPersonalDocuments;

public class GetAllPersonalDocumentsQueryHandler : IRequestHandler<GetAllPersonalDocumentsQuery, List<FarmXpert.Domain.Entities.PersonalDocument>>
{
    private readonly Domain.Interfaces.IPersonalDocumentRepository _personalDocumentRepository;
    public GetAllPersonalDocumentsQueryHandler(Domain.Interfaces.IPersonalDocumentRepository personalDocumentRepository)
    {
        _personalDocumentRepository = personalDocumentRepository;
    }
    public async Task<List<FarmXpert.Domain.Entities.PersonalDocument>> Handle(GetAllPersonalDocumentsQuery request, CancellationToken cancellationToken)
    {
        var personalDocuments = await _personalDocumentRepository.GetAllAsync(request.OwnerId, cancellationToken);
        return personalDocuments.ToList();
    }
}
