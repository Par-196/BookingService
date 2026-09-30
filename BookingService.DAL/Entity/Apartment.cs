using BookingService.DAL.Enums;

namespace BookingService.DAL.Entity;

public class Apartment
{
    public Guid Id { get; set; }
    public ApartmentTypes ApartmentType { get; set; }
    public string Location { get; set; } = null!;
    
    public ICollection<Booking> Bookings { get; set; } = [];
}