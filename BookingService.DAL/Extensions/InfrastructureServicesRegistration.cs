using BookingService.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.DAL.Extensions;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnectionString")));
        
        return services;
    }
}