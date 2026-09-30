using BookingService.BLL.Models.Enums;

namespace BookingService.BLL.Models;

public class ApartmentDTO
{
    public Guid Id { get; set; }
    public ApartmentTypes ApartmentType { get; set; }
    public string Location { get; set; } = null!;
}