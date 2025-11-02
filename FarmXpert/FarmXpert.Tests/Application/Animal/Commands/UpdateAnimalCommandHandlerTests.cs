using FarmXpert.Application.Animal.Commands.UpdateAnimal;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Animal.Commands;

public class UpdateAnimalCommandHandlerTests
{
    [Fact]
    public async Task Handle_NonExistentAnimal_ThrowsKeyNotFoundException()
    {
        // Arrange
        var mockRepo = new Mock<IAnimalRepository>();
        var handler = new UpdateAnimalCommandHandler(mockRepo.Object);
        var animal = new Domain.Entities.Animal { Id = Guid.NewGuid(), OwnerId = "owner123" };
        var command = new UpdateAnimalCommand(animal);

        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Animal?)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}