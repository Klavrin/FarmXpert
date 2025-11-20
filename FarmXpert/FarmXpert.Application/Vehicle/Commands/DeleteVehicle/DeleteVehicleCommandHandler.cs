using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.DeleteVehicle;

public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, FarmXpert.Domain.Entities.Vehicle?>
{
    private readonly IVehicleRepository _vehicleRepository;

    public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Vehicle?> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.OwnerId, request.Id, cancellationToken);
        if (vehicle == null)
        {
            return null;
        }

        await _vehicleRepository.DeleteAsync(request.Id, cancellationToken);
        return vehicle;
    }
}
