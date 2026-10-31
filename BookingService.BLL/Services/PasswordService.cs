using Microsoft.AspNetCore.Identity;

namespace BookingService.BLL.Services;

public class PasswordService
{
    private readonly PasswordHasher<object> _hasher = new();

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool VerifyPassword(string hash, string password)
    {
        var result = _hasher.VerifyHashedPassword(
            null!,
            hash,
            password);

        return result != PasswordVerificationResult.Failed;
    }
}