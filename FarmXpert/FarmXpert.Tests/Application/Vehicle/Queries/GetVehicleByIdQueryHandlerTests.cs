using FarmXpert.Application.Vehicle.Queries.GetVehicleById;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Vehicle.Queries;

public class GetVehicleByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExistingVehicle_ReturnsVehicle()
    {
        // Arrange
        var mockRepo = new Mock<IVehicleRepository>();
        var expectedVehicle = new Domain.Entities.Vehicle { Id = Guid.NewGuid(), OwnerId = "owner123" };
        mockRepo.Setup(r => r.GetByIdAsync(expectedVehicle.OwnerId, expectedVehicle.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedVehicle);

        var handler = new GetVehicleByIdQueryHandler(mockRepo.Object);
        var query = new GetVehicleByIdQuery(expectedVehicle.OwnerId, expectedVehicle.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().Be(expectedVehicle);
    }

    [Fact]
    public async Task Handle_NonExistentVehicle_ReturnsNull()
    {
        // Arrange
        var mockRepo = new Mock<IVehicleRepository>();
        mockRepo.Setup(r => r.GetByIdAsync("owner123", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Vehicle?)null);

        var handler = new GetVehicleByIdQueryHandler(mockRepo.Object);
        var query = new GetVehicleByIdQuery("owner123", Guid.NewGuid());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
