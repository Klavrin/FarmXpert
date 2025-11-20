using FarmXpert.Application.Field.Commands.DeleteField;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Field.Commands;

public class DeleteFieldCommandHandlerTests
{
    private readonly Mock<IFieldRepository> _mockRepo;
    private readonly DeleteFieldCommandHandler _handler;

    public DeleteFieldCommandHandlerTests()
    {
        _mockRepo = new Mock<IFieldRepository>();
        _handler = new DeleteFieldCommandHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_ExistingField_DeletesAndReturnsField()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var field = new Domain.Entities.Field { Id = fieldId, CropType = Domain.Enums.CropType.Garlic };
        var command = new DeleteFieldCommand("owner123", field.Id);

        _mockRepo.Setup(r => r.GetByIdAsync("owner123", field.Id, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(field);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(field.Id);
        _mockRepo.Verify(r => r.DeleteAsync(field.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentField_ReturnsNull()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var command = new DeleteFieldCommand("owner123", fieldId);

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Domain.Entities.Field?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
        _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
