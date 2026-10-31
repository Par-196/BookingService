using System.Text;
using BookingService.DAL.Context;
using BookingService.IntegrationTests.Configurations;
using BookingService.IntegrationTests.Seeder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Testcontainers.PostgreSql;

namespace BookingService.IntegrationTests.Infrastructure;

public class BookingApplicationWebFactory
    : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container =
        new PostgreSqlBuilder("postgres:17.6-alpine")
            .Build();

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureTestServices(services =>
        {
            // Replace production database settings.
            services.RemoveAll<
                DbContextOptions<BookingContext>>();

            services.RemoveAll<
                IDbContextOptionsConfiguration<BookingContext>>();

            services.AddDbContext<BookingContext>(options =>
                options.UseNpgsql(
                    _container.GetConnectionString()));

            // Use a test signing key while keeping the
            // real JWT Bearer authentication handler.
            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = TestJwtSettings.Issuer,

                            ValidateAudience = true,
                            ValidAudience = TestJwtSettings.Audience,

                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        TestJwtSettings.Key)),

                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero,

                            NameClaimType =
                                System.Security.Claims
                                    .ClaimTypes.NameIdentifier
                        };
                });

            services.AddScoped<TestDataSeeder>();
        });
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<BookingContext>();

        await context.Database.MigrateAsync();

        var seeder = scope.ServiceProvider
            .GetRequiredService<TestDataSeeder>();

        await seeder.SeedAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await base.DisposeAsync();
        await _container.DisposeAsync();
    }
}