using FarmXpert.Application.Animal.Commands.DeleteAnimal;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Animal.Commands;

public class DeleteAnimalCommandHandlerTests
{
    private readonly Mock<IAnimalRepository> _mockRepo;
    private readonly DeleteAnimalCommandHandler _handler;

    public DeleteAnimalCommandHandlerTests()
    {
        _mockRepo = new Mock<IAnimalRepository>();
        _handler = new DeleteAnimalCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ExistingAnimal_DeletesAndReturnsAnimal()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        var animal = new Domain.Entities.Animal { Id = animalId, Species = "Cow" };
        var command = new DeleteAnimalCommand("owner123", animalId);

        _mockRepo.Setup(r => r.GetByIdAsync("owner123", animalId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(animal);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(animalId);
        _mockRepo.Verify(r => r.DeleteAsync(animalId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentAnimal_ReturnsNull()
    {
        // Arrange
        var animalId = Guid.NewGuid();
        var command = new DeleteAnimalCommand("owner123", animalId);

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Domain.Entities.Animal?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
