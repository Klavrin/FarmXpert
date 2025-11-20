using FarmXpert.Application.Vehicle.Queries.GetAllVehicles;
using FarmXpert.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace FarmXpert.Tests.Application.Vehicle.Queries;
public class GetAllVehiclesQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsAllVehiclesForOwner()
    {
        // Arrange
        var mockRepo = new Mock<IVehicleRepository>();
        var expectedVehicles = new List<Domain.Entities.Vehicle>
        {
            new Domain.Entities.Vehicle { Id = Guid.NewGuid(), OwnerId = "owner123" },
            new Domain.Entities.Vehicle { Id = Guid.NewGuid(), OwnerId = "owner123" }
        };
        mockRepo.Setup(r => r.GetAllAsync("owner123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedVehicles);

        var handler = new GetAllVehiclesQueryHandler(mockRepo.Object);
        var query = new GetAllVehiclesQuery("owner123");

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(expectedVehicles);
    }
}
