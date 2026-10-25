using BookingService.BLL.Models;
using BookingService.DAL.Entity;

namespace BookingService.BLL.Extensions.Mapping;

public static class ApartmentMappingExtensions
{
    
    
        public static ApartmentDTO ToDto(this Apartment apartment)
        {
            return new ApartmentDTO
            {
                Id = apartment.Id,
                
                ApartmentType = Enum.Parse<Enums.ApartmentTypes>(
                    apartment.ApartmentType.ToString()),

                Location = apartment.Location
            };
        }

        public static Apartment ToEntity(this ApartmentDTO apartmentDto)
        {
            return new Apartment
            {
                Id = apartmentDto.Id,

                ApartmentType = Enum.Parse<BookingService.DAL.Enums.ApartmentTypes>(
                    apartmentDto.ApartmentType.ToString()),

                Location = apartmentDto.Location
            };
        }
}