using BookingService.BLL.Services;
using BookingService.DAL.Interfaces;

namespace BookingService.BLL.DataMigrations;

public class PasswordMigrationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordService _passwordService;

    public PasswordMigrationService(
        IUnitOfWork unitOfWork,
        PasswordService  passwordService)
    {
        _unitOfWork = unitOfWork;
        _passwordService = passwordService;
    }

    public async Task MigratePasswordsAsync()
    {
        var users = await _unitOfWork.UserRepository
            .GetUsersWithoutPasswordHashAsync();
        
        foreach (var user in users)
        {
            user.Password =
                _passwordService.HashPassword(user.Password);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}