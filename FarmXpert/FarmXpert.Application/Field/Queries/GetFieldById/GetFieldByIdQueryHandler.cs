using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Field.Queries.GetFieldById;
public class GetFieldByIdQueryHandler : IRequestHandler<GetFieldByIdQuery, FarmXpert.Domain.Entities.Field?>
{
    private readonly IFieldRepository _fieldRepository;

    public GetFieldByIdQueryHandler(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Field?> Handle(GetFieldByIdQuery request, CancellationToken cancellationToken)
        => await _fieldRepository.GetByIdAsync(request.Id, cancellationToken);
}