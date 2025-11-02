using FarmXpert.Application.Animal.Commands.CreateAnimal;
using FarmXpert.Application.Field.Commands.CreateField;
using FarmXpert.Domain.Enums;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Field.Commands;

public class CreateFieldCommandHandlerTests
{
    private readonly Mock<IFieldRepository> _mockRepo;
    private readonly CreateFieldCommandHandler _handler;

    public CreateFieldCommandHandlerTests()
    {
        _mockRepo = new Mock<IFieldRepository>();
        _handler = new CreateFieldCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesField()
    {
        // Arrange
        var command = new CreateFieldCommand(
            BusinessId: Guid.NewGuid(),
            CropType: CropType.Wheat,
            OtherCropType: "",
            SoilType: SoilType.Vertisol,
            OtherSoilType: "",
            Fertilizer: FertilizerType.Other,
            OtherFertilizer: "",
            Herbicide: HerbicideType.Other,
            OtherHerbicide: "",
            Coords: new List<double[]> { new double[] { 0, 0 }, new double[] { 1, 1 } },
            OwnerId: "owner123");

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Field>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.BusinessId.Should().Be(command.BusinessId);
        result.CropType.Should().Be(command.CropType);
        result.SoilType.Should().Be(command.SoilType);
        result.Fertilizer.Should().Be(command.Fertilizer);
        result.Herbicide.Should().Be(command.Herbicide);
        result.Coords.Should().BeEquivalentTo(command.Coords);
        result.OwnerId.Should().Be(command.OwnerId);

        _mockRepo.Verify(r => r.CreateAsync(It.IsAny<Domain.Entities.Field>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var command = new CreateFieldCommand(
            BusinessId: Guid.NewGuid(),
            CropType: CropType.Wheat,
            OtherCropType: "",
            SoilType: SoilType.Vertisol,
            OtherSoilType: "",
            Fertilizer: FertilizerType.Other,
            OtherFertilizer: "",
            Herbicide: HerbicideType.Other,
            OtherHerbicide: "",
            Coords: new List<double[]> { new double[] { 0, 0 }, new double[] { 1, 1 } },
            OwnerId: "owner123");

        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Domain.Entities.Field>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new Exception("Database error"));

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
    }
}
