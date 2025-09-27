using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Field.Commands.DeleteField;
public class DeleteFieldCommandHandler : IRequestHandler<DeleteFieldCommand, FarmXpert.Domain.Entities.Field?>
{
    private readonly IFieldRepository _fieldRepository;

    public DeleteFieldCommandHandler(IFieldRepository fieldRepository)
    {
        _fieldRepository = fieldRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Field?> Handle(DeleteFieldCommand request, CancellationToken cancellationToken)
    {
        var field = await _fieldRepository.GetByIdAsync(request.Id, cancellationToken);
        if (field == null)
            return null;

        await _fieldRepository.DeleteAsync(request.Id, cancellationToken);
        return field;
    }
}