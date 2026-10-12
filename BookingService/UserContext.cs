using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookingService;

public class UserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetId()
    {
        var userIdClaim = _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        return !Guid.TryParse(userIdClaim, out var userId) 
            ? throw new UnauthorizedAccessException("User id claim is missing.") 
            : userId;
    }
}