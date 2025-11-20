using FarmXpert.Application.Field.Commands.UpdateField;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Field.Commands;

public class UpdateFieldCommandHandlerTests
{
    [Fact]
    public async Task Handle_NonExistentField_ThrowsKeyNotFoundException()
    {
        // Arrange
        var mockRepo = new Mock<IFieldRepository>();
        var handler = new UpdateFieldCommandHandler(mockRepo.Object);
        var field = new Domain.Entities.Field { Id = Guid.NewGuid(), OwnerId = "owner123" };
        var command = new UpdateFieldCommand(field);

        mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Field?)null);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
