using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using BookingService.IntegrationTests.Configurations;
using Microsoft.IdentityModel.Tokens;

namespace BookingService.IntegrationTests.Helpers;

public static class JwtTokenHelper
{
    public static string GenerateToken(Guid userId)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(TestJwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: TestJwtSettings.Issuer,
            audience: TestJwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public static void Authorize(
        HttpClient client,
        Guid userId)
    {
        var token = GenerateToken(userId);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);
    }

    public static HttpRequestMessage CreateAuthorizedRequest(
        HttpMethod method,
        string url,
        Guid userId)
    {
        var request = new HttpRequestMessage(method, url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                GenerateToken(userId));

        return request;
    }
}