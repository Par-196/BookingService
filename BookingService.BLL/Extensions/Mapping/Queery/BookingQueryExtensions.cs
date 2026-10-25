using BookingService.BLL.Query;

namespace BookingService.BLL.Extensions.Mapping.Queery;

public static class BookingQueryExtensions
{
        public static BookingQueryDTO ToDto(this BookingQuery bookingQueryDto)
        {
            return new BookingQueryDTO
            {
                StartDate = bookingQueryDto.StartDate,
                EndDate = bookingQueryDto.StartDate,
                ApartmentId = bookingQueryDto.ApartmentId,
                Skip = bookingQueryDto.Skip,
                Take = bookingQueryDto.Take,
            };
        }
        
        public static BookingQuery ToQuery(this BookingQueryDTO bookingQueryDto)
        {
            return new BookingQuery
            {
                StartDate = bookingQueryDto.StartDate,
                EndDate = bookingQueryDto.StartDate,
                ApartmentId = bookingQueryDto.ApartmentId,
                Skip = bookingQueryDto.Skip,
                Take = bookingQueryDto.Take,
            };
        }
}