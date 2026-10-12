using BookingService.BLL.Enums;

namespace BookingService.BLL.DTO;

public class UserDTO
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    
    public UserRoles  Role { get; set; } 
}
