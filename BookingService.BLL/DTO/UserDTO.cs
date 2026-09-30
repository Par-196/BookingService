using BookingService.DAL.Entity;

namespace BookingService.BLL.Models;

public class UserDTO
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}