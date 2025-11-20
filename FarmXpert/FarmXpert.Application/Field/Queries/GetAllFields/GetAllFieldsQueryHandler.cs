using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Field.Queries.GetAllFields;
public class GetAllFieldsQueryHandler : IRequestHandler<GetAllFieldsQuery, List<FarmXpert.Domain.Entities.Field>>
{
    private readonly IFieldRepository _fieldRepository;

    public GetAllFieldsQueryHandler(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public async Task<List<FarmXpert.Domain.Entities.Field>> Handle(GetAllFieldsQuery request, CancellationToken cancellationToken)
    {
        var fields = await _fieldRepository.GetAllAsync(request.OwnerId, cancellationToken);
        return fields.ToList();
    }
}
