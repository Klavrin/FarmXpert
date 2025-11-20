using FarmXpert.Application.Animal.Queries.GetAllAnimals;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Animal.Queries;
public class GetAllAnimalsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsAllAnimalsForOwner()
    {
        // Arrange
        var mockRepo = new Mock<IAnimalRepository>();
        var expectedAnimals = new List<Domain.Entities.Animal>
        {
            new Domain.Entities.Animal { Id = Guid.NewGuid(), OwnerId = "owner123" },
            new Domain.Entities.Animal { Id = Guid.NewGuid(), OwnerId = "owner123" }
        };
        mockRepo.Setup(r => r.GetAllAsync("owner123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedAnimals);

        var handler = new GetAllAnimalsQueryHandler(mockRepo.Object);
        var query = new GetAllAnimalsQuery("owner123");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedAnimals);
    }
}
