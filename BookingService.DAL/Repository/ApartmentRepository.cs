using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingService.DAL.Repository;

public class ApartmentRepository : IApartmentRepository
{
    private readonly BookingContext _context;

    public ApartmentRepository(BookingContext context)
    {
        _context = context;
    }

    public async Task<Apartment?> GetApartmentByIdAsync(Guid id)
    {
        return await _context.Apartments.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Apartment>> GetAllApartmentsAsync()
    {
        return await _context.Apartments.ToListAsync();
    }

    public async Task<bool> CreateApartmentAsync(Apartment apartment)
    {
        await _context.Apartments.AddAsync(apartment);
        return true;
    }

    public async Task<bool> UpdateApartmentAsync(Apartment apartment)
    {
        var entity = await GetApartmentByIdAsync(apartment.Id);

        if (entity is null)
        {
            return false;
        }
        
        entity.ApartmentType = apartment.ApartmentType;
        entity.Location = apartment.Location;
        
        return true;
    }

    public async Task<bool> DeleteApartmentByIdAsync(Guid id)
    {
        var apartment = await GetApartmentByIdAsync(id);

        if (apartment is null) return false;

        _context.Apartments.Remove(apartment);
        return true;
    }
}