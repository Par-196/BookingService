namespace BookingService.DAL.Entity;

public class Booking
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Guid ApartmentId { get; set; }
    public Guid UserId { get; set; }
    
    public User User { get; set; } = null!;
    public Apartment Apartment { get; set; } = null!;
}