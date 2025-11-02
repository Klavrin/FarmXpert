using Moq;
using FarmXpert.Application.Field.Queries.GetFieldById;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;

namespace FarmXpert.Tests.Application.Field.Queries;

public class GetFieldByIdQueryHandlerTests
{
[Fact]
public async Task Handle_ExistingField_ReturnsField()
{
    // Arrange
    var mockRepo = new Mock<IFieldRepository>();
    var expectedField = new Domain.Entities.Field { Id = Guid.NewGuid(), OwnerId = "owner123" };
    mockRepo.Setup(r => r.GetByIdAsync(expectedField.OwnerId, expectedField.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedField);

    var handler = new GetFieldByIdQueryHandler(mockRepo.Object);
    var query = new GetFieldByIdQuery(expectedField.OwnerId, expectedField.Id);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().Be(expectedField);
}

[Fact]
public async Task Handle_NonExistentField_ReturnsNull()
{
    // Arrange
    var mockRepo = new Mock<IFieldRepository>();
    mockRepo.Setup(r => r.GetByIdAsync("owner123", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Field?)null);

    var handler = new GetFieldByIdQueryHandler(mockRepo.Object);
    var query = new GetFieldByIdQuery("owner123", Guid.NewGuid());

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().BeNull();
}
}