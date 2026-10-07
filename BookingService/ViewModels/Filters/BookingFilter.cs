namespace BookingService.ViewModels.Filters;

public class BookingFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? ApartmentId { get; set; }

    public int Skip { get; set; } = 0;
}