using MediatR;

namespace FarmXpert.Application.ApplicationDocument.Queries.GetApplicationDocumentById;

public class GetApplicationDocumentByIdQueryHandler : IRequestHandler<GetApplicationDocumentByIdQuery, FarmXpert.Domain.Entities.ApplicationDocument?>
{
    private readonly Domain.Interfaces.IApplicationDocumentRepository _applicationDocumentRepository;
    public GetApplicationDocumentByIdQueryHandler(Domain.Interfaces.IApplicationDocumentRepository applicationDocumentRepository)
    {
        _applicationDocumentRepository = applicationDocumentRepository;
    }
    public async Task<FarmXpert.Domain.Entities.ApplicationDocument> Handle(GetApplicationDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var applicationDocument = await _applicationDocumentRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        return applicationDocument;
    }
}
