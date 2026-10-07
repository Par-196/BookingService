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

        var apartmentDTO = new ApartmentDTO
        {
            Id = apartmentModel.Id,
            ApartmentType = Enum.Parse<ApartmentTypes>(apartmentModel.ApartmentType.ToString()),
            Location = apartmentModel.Location
        };
        
        return apartmentDTO;
    }

    public async Task<List<ApartmentDTO>> GetAllApartmentsAsync()
    {
        var listApartmentsDto = await _context.Apartments.Select(a => new ApartmentDTO
        {
            Id = a.Id,
            ApartmentType = Enum.Parse<ApartmentTypes>(a.ApartmentType.ToString()),
            Location = a.Location
        }).ToListAsync();
        
        return listApartmentsDto;
    }

    public async Task<bool> CreateApartmentAsync(ApartmentDTO apartmentDto)
    {
        var apartmentsModel = new Apartment
        {
            Id = Guid.NewGuid(),
            ApartmentType = Enum.Parse<DAL.Enums.ApartmentTypes>(apartmentDto.ApartmentType.ToString()),
            Location = apartmentDto.Location
        };

        await _context.Apartments.AddAsync(apartmentsModel);
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
        _logger.LogInformation("apartmentModel not null");
        apartmentModel.Location = apartmentDto.Location;
        apartmentModel.ApartmentType = Enum.Parse<DAL.Enums.ApartmentTypes>(apartmentDto.ApartmentType.ToString());
        _logger.LogInformation("apartmentModel changed");
        var result = await _context.SaveChangesAsync();
        _logger.LogInformation("apartmentModel saved");
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