using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.CreateVehicle;

public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, FarmXpert.Domain.Entities.Vehicle>
{
    private readonly IVehicleRepository _vehicleRepository;

    public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Vehicle> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var vehicle = new FarmXpert.Domain.Entities.Vehicle
        {
            Id = Guid.NewGuid(),
            BusinessId = request.BusinessId,
            VehicleGroupId = request.VehicleGroupId,
            VehicleType = request.VehicleType,
            FabricationDate = request.FabricationDate,
            Brand = request.Brand,
            OwnerId = request.OwnerId
        };

        await _vehicleRepository.CreateAsync(vehicle, cancellationToken);
        return vehicle;
    }
}