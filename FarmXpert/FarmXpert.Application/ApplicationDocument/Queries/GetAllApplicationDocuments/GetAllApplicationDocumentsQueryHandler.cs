using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Queries.GetAllApplicationDocuments;

public class GetAllApplicationDocumentsQueryHandler : IRequestHandler<GetAllApplicationDocumentsQuery, List<FarmXpert.Domain.Entities.ApplicationDocument>>
{
    private readonly Domain.Interfaces.IApplicationDocumentRepository _applicationDocumentRepository;
    public GetAllApplicationDocumentsQueryHandler(Domain.Interfaces.IApplicationDocumentRepository applicationDocumentRepository)
    {
        _applicationDocumentRepository = applicationDocumentRepository;
    }
    public async Task<List<FarmXpert.Domain.Entities.ApplicationDocument>> Handle(GetAllApplicationDocumentsQuery request, CancellationToken cancellationToken)
    {
        var applicationDocuments = await _applicationDocumentRepository.GetAllAsync(request.OwnerId, cancellationToken);
        return applicationDocuments.ToList();
    }
}
