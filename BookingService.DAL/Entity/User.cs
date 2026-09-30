using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using BookingService.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Entity;

public class User
{
    public Guid Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public UserRoles  Role { get; set; } 
    
    public ICollection<Booking> Bookings { get; set; } = [];
}