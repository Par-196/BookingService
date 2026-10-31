namespace BookingService.IntegrationTests.Configurations;


public static class TestJwtSettings
{
    public const string Issuer = "IntegrationTests";
    public const string Audience = "IntegrationTests.Api";

    public const string Key =
        "IntegrationTestsSigningKey12345678901234567890";
}