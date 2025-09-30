using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Vehicle.Commands.UpdateVehicle;

public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, FarmXpert.Domain.Entities.Vehicle?>
{
    private readonly IVehicleRepository _vehicleRepository;

    public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Vehicle?> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
    {
        var existingVehicle = await _vehicleRepository.GetByIdAsync(request.Vehicle.OwnerId,request.Vehicle.Id, cancellationToken);
        if (existingVehicle == null)
        {
            return null;
        }

        existingVehicle.BusinessId = request.Vehicle.BusinessId;
        existingVehicle.VehicleGroupId = request.Vehicle.VehicleGroupId;
        existingVehicle.VehicleType = request.Vehicle.VehicleType;
        existingVehicle.FabricationDate = request.Vehicle.FabricationDate;
        existingVehicle.Brand = request.Vehicle.Brand;

        await _vehicleRepository.UpdateAsync(existingVehicle, cancellationToken);
        return existingVehicle;
    }
}