namespace BookingService.BLL.Query;

public class BookingQueryDTO
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? ApartmentId { get; set; }

    public int Skip { get; set; }
    public int Take { get; set; }
}