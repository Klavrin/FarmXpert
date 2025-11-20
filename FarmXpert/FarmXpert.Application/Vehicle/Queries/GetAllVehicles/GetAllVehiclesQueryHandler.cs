using FarmXpert.Domain.Entities;
using FarmXpert.Domain.Interfaces;
using MediatR;

namespace FarmXpert.Application.Vehicle.Queries.GetAllVehicles;

public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, List<FarmXpert.Domain.Entities.Vehicle>>
{
    private readonly IVehicleRepository _vehicleRepository;

    public GetAllVehiclesQueryHandler(IVehicleRepository vehicleRepository)
    {
        _vehicleRepository = vehicleRepository;
    }

    public async Task<List<FarmXpert.Domain.Entities.Vehicle>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetAllAsync(request.OwnerId, cancellationToken);
        return vehicles.ToList();
    }
}
