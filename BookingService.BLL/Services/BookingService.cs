using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Extensions.Mapping.Extensions;
using BookingService.BLL.Extensions.Mapping.Queery;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.BLL.Query;
using BookingService.DAL.Context;
using BookingService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace BookingService.BLL.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<BookingDTO?> GetBookingByIdAsync(Guid id)
    {
        return (await _unitOfWork.BookingRepository.GetBookingByIdAsync(id))?
            .ToDto();
    }

    public async Task<List<BookingDTO>?> GetAllBookingsAsync()
    {
        return (await _unitOfWork.BookingRepository.GetAllBookingsAsync())
            .Select(b => b.ToDto())?
            .ToList();
    }

    public async Task<List<BookingDTO>?> GetBookingsByUserIdAsync(Guid userId)
    {
        return (await _unitOfWork.BookingRepository.GetBookingsByUserIdAsync(userId))?
            .Select(b => b.ToDto())?
            .ToList();
    }

    public async Task<List<BookingDTO>?> GetFilteredAndPaginatedBookingsAsync(BookingQueryDTO filter)
    {
        return (await _unitOfWork.BookingRepository
                .GetFilteredAndPaginatedBookingsAsync(filter.ToQuery()))?
            .Select(b => b.ToDto())
            .ToList();
    }

    public async Task<List<BookingDTO>?> GetBookingsByApartmentIdAsync(Guid apartmentId)
    {
        return (await _unitOfWork.BookingRepository.GetBookingsByApartmentIdAsync(apartmentId))
            .Select(b => b.ToDto())?
            .ToList();
    }

    public async Task<bool> CreateBookingAsync(BookingDTO? bookingDto)
    {
        return bookingDto is not null 
               && (await _unitOfWork.BookingRepository.CreateBookingAsync(bookingDto.ToEntity()) 
               && await _unitOfWork.SaveChangesAsync() > 0);
    }

    public async Task<bool> UpdateBookingAsync(BookingDTO? bookingDto)
    {
        return bookingDto is not null 
               && (await _unitOfWork.BookingRepository.UpdateBookingAsync(bookingDto.ToEntity()) 
                   && await _unitOfWork.SaveChangesAsync() > 0);
    }

    public async Task<bool> DeleteBookingByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            return false;

        var deleted = await _unitOfWork.BookingRepository
            .DeleteBookingByIdAsync(id);

        if (!deleted)
            return false;

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    // private async Task<bool> IsApartmentAvailableAsync(Guid apartmentId, DateTime startDate, DateTime endDate)
    // {
    //     return await _unitOfWork.BookingRepository.HasBookingConflictAsync(apartmentId, startDate, endDate);
    // }
}