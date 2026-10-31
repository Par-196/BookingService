namespace BookingService.IntegrationTests.Seeder.TestData;

public static class TestBookings
{
    public static class DefaultBooking
    {
        public static readonly Guid Id =
            Guid.Parse("55555555-5555-5555-5555-555555555555");

        public static DateTime StartDate => DateTime.UtcNow;
        public static DateTime EndDate => DateTime.UtcNow;

        public static readonly Guid ApartmentId =
            TestApartments.ApartmentForBooking.Id;

        public static readonly Guid UserId =
            TestUsers.UserForBooking.Id;
    }

    public static class BookingToDelete
    {
        public static readonly Guid Id =
            Guid.Parse("66666666-6666-6666-6666-666666666666");

        public static DateTime StartDate => DateTime.UtcNow;
        public static DateTime EndDate => DateTime.UtcNow;

        public static readonly Guid ApartmentId =
            TestApartments.ApartmentForBooking.Id;

        public static readonly Guid UserId =
            TestUsers.UserForBooking.Id;
    }
}