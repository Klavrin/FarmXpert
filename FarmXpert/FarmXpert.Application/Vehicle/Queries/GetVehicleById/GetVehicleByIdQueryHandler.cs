using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Vehicle.Queries.GetVehicleById;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, FarmXpert.Domain.Entities.Vehicle?>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetVehicleByIdQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<FarmXpert.Domain.Entities.Vehicle?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _vehicleRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}