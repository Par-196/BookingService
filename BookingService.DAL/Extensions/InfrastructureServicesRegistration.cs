using BookingService.DAL.Context;
using BookingService.DAL.Interfaces;
using BookingService.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BookingService.DAL.UnitOfWork;

namespace BookingService.DAL.Extensions;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookingContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnectionString")));

        
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        return services;
    }
}