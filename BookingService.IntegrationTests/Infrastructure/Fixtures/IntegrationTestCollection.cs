namespace BookingService.IntegrationTests.Infrastructure.Fixtures;

[CollectionDefinition(
    "IntegrationTests",
    DisableParallelization = true)]
public class IntegrationTestCollection
    : ICollectionFixture<BookingApplicationWebFactory>
{
}