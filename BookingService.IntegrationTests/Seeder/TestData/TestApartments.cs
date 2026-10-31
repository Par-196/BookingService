using BookingService.DAL.Enums;

namespace BookingService.IntegrationTests.Seeder.TestData;

public static class TestApartments
{
    public static class DefaultApartment
    {
        public static readonly Guid Id =
            Guid.Parse("33333333-3333-3333-3333-333333333333");

        public const ApartmentTypes ApartmentType = ApartmentTypes.OneBedroom;

        public const string Location = "Berlin, Germany";
    }

    public static class ApartmentToDelete
    {
        public static readonly Guid Id =
            Guid.Parse("44444444-4444-4444-4444-444444444444");

        public const ApartmentTypes ApartmentType = ApartmentTypes.OneBedroom;

        public const string Location = "Hamburg, Germany";
    }
    
    public static class ApartmentForBooking
    {
        public static readonly Guid Id =
            Guid.Parse("77777777-7777-7777-7777-777777777777");

        public const ApartmentTypes ApartmentType =
            ApartmentTypes.OneBedroom;

        public const string Location = "Munich, Germany";
    }
}