using BookingService.DAL.Entity;

namespace BookingService.DAL.Interfaces;

public interface IApartmentRepository
{
    Task<Apartment?> GetApartmentByIdAsync(Guid id);
    
    Task<List<Apartment>> GetAllApartmentsAsync();
    
    Task<bool> CreateApartmentAsync(Apartment apartmentDto);
    
    Task<bool> UpdateApartmentAsync(Apartment apartmentDto);
    
    Task<bool> DeleteApartmentByIdAsync(Guid id);
}

