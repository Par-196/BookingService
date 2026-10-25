using BookingService.BLL.Models;

namespace BookingService.BLL.Interfaces;

public interface IApartmentService
{
    public Task<ApartmentDTO?> GetApartmentByIdAsync(Guid id);
    
    public Task<List<ApartmentDTO>> GetAllApartmentsAsync();
    
    public Task<bool> CreateApartmentAsync(ApartmentDTO apartmentDto);
    
    public Task<bool> UpdateApartmentAsync(ApartmentDTO apartmentDto);
    
    public Task<bool> DeleteApartmentByIdAsync(Guid id);
}