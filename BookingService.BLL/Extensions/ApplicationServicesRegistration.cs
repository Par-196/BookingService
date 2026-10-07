using BookingService.BLL.Interfaces;
using BookingService.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.BLL.Extensions;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //AddScoped - during Http call
        //AddTransient - every time you access it
        //AddSingleton - during whole application lifetime\
        
        services.AddScoped<IBookingService, Services.BookingService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IApartmentService, ApartmentService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}