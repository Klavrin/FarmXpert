using FarmXpert.Application.Animal.Commands.CreateAnimal;
using FarmXpert.Domain.Enums;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Animal.Commands;

public class GetAllAnimalsQueryHandler
{
    private readonly Mock<IAnimalRepository> _mockRepo;
    private readonly CreateAnimalCommandHandler _handler;

    public GetAllAnimalsQueryHandler()
    {
        _mockRepo = new Mock<IAnimalRepository>();
        _handler = new CreateAnimalCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesAnimal()
    {
        // Arrange
        var command = new CreateAnimalCommand(
            Guid.NewGuid(),
            "Cow",
            Sex.Female,
            DateTime.Now.AddYears(-2),
            "owner123"
        );

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Animal>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Species.Should().Be("Cow");
        result.OwnerId.Should().Be("owner123");
        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Domain.Entities.Animal>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var command = new CreateAnimalCommand(
            Guid.NewGuid(), 
            "Cow", 
            Sex.Female, 
            DateTime.Now, 
            "owner123");

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Animal>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}