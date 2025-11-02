using FarmXpert.Application.Vehicle.Commands.DeleteVehicle;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Vehicle.Commands;

public class DeleteVehicleCommandHandlerTests
{
    private readonly Mock<IVehicleRepository> _mockRepo;
    private readonly DeleteVehicleCommandHandler _handler;

    public DeleteVehicleCommandHandlerTests()
    {
        _mockRepo = new Mock<IVehicleRepository>();
        _handler = new DeleteVehicleCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ExistingVehicle_DeletesAndReturnsVehicle()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var vehicle = new Domain.Entities.Vehicle { Id = vehicleId, Brand = "Toyota" };
        var command = new DeleteVehicleCommand("owner123", vehicle.Id);

        _mockRepo.Setup(r => r.GetByIdAsync("owner123", vehicle.Id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(vehicle.Id);
        _mockRepo.Verify(r => r.DeleteAsync(vehicle.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentVehicle_ReturnsNull()
    {
        // Arrange
        var vehicleId = Guid.NewGuid();
        var command = new DeleteVehicleCommand("owner123", vehicleId);

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Domain.Entities.Vehicle?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}