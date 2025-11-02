using FarmXpert.Application.Vehicle.Commands.CreateVehicle;
using FarmXpert.Domain.Enums;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Vehicle.Commands;

public class CreateVehicleCommandHandlerHandler
{
    private readonly Mock<IVehicleRepository> _mockRepo;
    private readonly CreateVehicleCommandHandler _handler;

    public CreateVehicleCommandHandlerHandler()
    {
        _mockRepo = new Mock<IVehicleRepository>();
        _handler = new CreateVehicleCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesVehicle()
    {
        // Arrange
        var command = new CreateVehicleCommand(
            OwnerId: "owner123",
            BusinessId: Guid.NewGuid(),
            VehicleGroupId: Guid.NewGuid(),
            VehicleType: "Truck",
            FabricationDate: 2018,
            Brand: "Volvo");

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Vehicle>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.OwnerId.Should().Be(command.OwnerId);
        result.BusinessId.Should().Be(command.BusinessId);
        result.VehicleGroupId.Should().Be(command.VehicleGroupId);
        result.VehicleType.Should().Be(command.VehicleType);
        result.FabricationDate.Should().Be(command.FabricationDate);
        result.Brand.Should().Be(command.Brand);

        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Domain.Entities.Vehicle>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var command = new CreateVehicleCommand(
            OwnerId: "owner123",
            BusinessId: Guid.NewGuid(),
            VehicleGroupId: Guid.NewGuid(),
            VehicleType: "Truck",
            FabricationDate: 2018,
            Brand: "Volvo");

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Vehicle>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}