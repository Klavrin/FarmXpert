using FarmXpert.Application.Vehicle.Commands.UpdateVehicle;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Vehicle.Commands;

public class UpdateVehicleCommandHandlerTests
{
    [Fact]
    public async Task Handle_NonExistentVehicle_ThrowsKeyNotFoundException()
    {
        // Arrange
        var mockRepo = new Mock<IVehicleRepository>();
        var handler = new UpdateVehicleCommandHandler(mockRepo.Object);
        var vehicle = new Domain.Entities.Vehicle { Id = Guid.NewGuid(), OwnerId = "owner123" };
        var command = new UpdateVehicleCommand(vehicle);

        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Vehicle?)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
