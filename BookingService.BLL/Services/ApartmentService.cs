using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Enums;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookingService.BLL.Services;

public class ApartmentService : IApartmentService
{
    private BookingContext _context;
    
    private ILogger<ApartmentService> _logger;
    
    public ApartmentService(BookingContext context, ILogger<ApartmentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ApartmentDTO?> GetApartmentByIdAsync(Guid id)
    {
        var apartmentModel = await _context.Apartments.FindAsync(id);

        if(apartmentModel is null)
        {
            return null;
        }
        
        return apartmentModel.ToDto();;
    }

    public async Task<List<ApartmentDTO>> GetAllApartmentsAsync()
    {
        var listApartmentsDto = await _context.Apartments
            .Select(a => a.ToDto())
            .ToListAsync();
        
        return listApartmentsDto;
    }

    public async Task<bool> CreateApartmentAsync(ApartmentDTO apartmentDto)
    {
        await _context.Apartments.AddAsync(apartmentDto.ToEntity());
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateApartmentAsync(ApartmentDTO apartmentDto)
    {
        var apartmentModel = await _context.Apartments.FindAsync(apartmentDto.Id);
        
        if (apartmentModel is null)
        {
            return false;
        }
        
        apartmentModel.Location = apartmentDto.Location;
        apartmentModel.ApartmentType = Enum.Parse<DAL.Enums.ApartmentTypes>(apartmentDto.ApartmentType.ToString());
       
        var result = await _context.SaveChangesAsync();
        
        return result > 0;
    }

    public async Task<bool> DeleteApartmentByIdAsync(Guid id)
    {
        var apartmentModel = await _context.Apartments.FindAsync(id);

        if (apartmentModel is null)
        {
            return false;
        }

        _context.Remove(apartmentModel);
        
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }
}