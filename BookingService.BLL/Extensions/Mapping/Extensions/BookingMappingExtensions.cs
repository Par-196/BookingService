using BookingService.BLL.Models;
using BookingService.DAL.Entity;

namespace BookingService.BLL.Extensions.Mapping.Extensions;

public static class BookingMappingExtensions
{
       
       public static BookingDTO ToDto(this Booking booking)
       {
              return  new BookingDTO
              {
                     Id = booking.Id,
                     StartDate = booking.StartDate,
                     EndDate = booking.EndDate,
                     ApartmentId = booking.ApartmentId,
                     UserId = booking.UserId,
              };
       }
       
       public static Booking ToEntity(this BookingDTO bookingDto)
       {
              return  new Booking
              {
                     Id = bookingDto.Id,
                     StartDate = bookingDto.StartDate,
                     EndDate = bookingDto.EndDate,
                     ApartmentId = bookingDto.ApartmentId,
                     UserId = bookingDto.UserId,
              };
       }
}