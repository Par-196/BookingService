using BookingService.IntegrationTests.Helpers;

namespace BookingService.IntegrationTests.Infrastructure;

public abstract class IntegrationTestBase : IDisposable
{
    protected BookingApplicationWebFactory Factory { get; }
    protected HttpClient Client { get; }

    protected IntegrationTestBase(
        BookingApplicationWebFactory factory)
    {
        Factory = factory;
        Client = factory.CreateClient();
    }

    protected void Authorize(Guid userId)
    {
        JwtTokenHelper.Authorize(Client, userId);
    }

    protected Task<HttpClient>
        CreateAuthorizedClientAsync(Guid userId)
    {
        var client = Factory.CreateClient();

        JwtTokenHelper.Authorize(client, userId);

        return Task.FromResult(client);
    }

    public void Dispose()
    {
        Client.Dispose();
    }
}