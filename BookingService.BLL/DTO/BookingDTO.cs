namespace BookingService.BLL.Models;

public class BookingDTO
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public Guid ApartmentId { get; set; }
    public Guid UserId { get; set; }
}