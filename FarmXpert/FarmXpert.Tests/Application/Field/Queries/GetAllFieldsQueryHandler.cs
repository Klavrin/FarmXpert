using Moq;
using FarmXpert.Application.Field.Queries.GetAllFields;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;

namespace FarmXpert.Tests.Application.Field.Queries;
public class GetAllFieldsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsAllFieldsForOwner()
    {
        // Arrange
        var mockRepo = new Mock<IFieldRepository>();
        var expectedFields = new List<Domain.Entities.Field>
        {
            new Domain.Entities.Field { Id = Guid.NewGuid(), OwnerId = "owner123" },
            new Domain.Entities.Field { Id = Guid.NewGuid(), OwnerId = "owner123" }
        };
        mockRepo.Setup(r => r.GetAllAsync("owner123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFields);

        var handler = new GetAllFieldsQueryHandler(mockRepo.Object);
        var query = new GetAllFieldsQuery("owner123");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedFields);
    }
}