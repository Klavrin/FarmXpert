using Moq;
using FarmXpert.Application.Animal.Queries.GetAnimalById;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;

namespace FarmXpert.Tests.Application.Animal.Queries;

public class GetAnimalByIdQueryHandlerTests
{
[Fact]
public async Task Handle_ExistingAnimal_ReturnsAnimal()
{
    // Arrange
    var mockRepo = new Mock<IAnimalRepository>();
    var expectedAnimal = new Domain.Entities.Animal { Id = Guid.NewGuid(), OwnerId = "owner123" };
    mockRepo.Setup(r => r.GetByIdAsync(expectedAnimal.OwnerId, expectedAnimal.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAnimal);

    var handler = new GetAnimalByIdQueryHandler(mockRepo.Object);
    var query = new GetAnimalByIdQuery(expectedAnimal.OwnerId, expectedAnimal.Id);

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().Be(expectedAnimal);
}

[Fact]
public async Task Handle_NonExistentAnimal_ReturnsNull()
{
    // Arrange
    var mockRepo = new Mock<IAnimalRepository>();
    mockRepo.Setup(r => r.GetByIdAsync("owner123", It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Animal?)null);

    var handler = new GetAnimalByIdQueryHandler(mockRepo.Object);
    var query = new GetAnimalByIdQuery("owner123", Guid.NewGuid());

    // Act
    var result = await handler.Handle(query, CancellationToken.None);

    // Assert
    result.Should().BeNull();
}
}