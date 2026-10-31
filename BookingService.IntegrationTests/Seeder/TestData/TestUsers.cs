namespace BookingService.IntegrationTests.Seeder.TestData;

public static class TestUsers
{
    public static class DefaultUser
    {
        public static readonly Guid Id =
            Guid.Parse("11111111-1111-1111-1111-111111111111");

        public const string Email = "user1@example.com";
        public const string FirstName = "loginUser";
        public const string Password = "LoginUser123!";
    }

    public static class UserToDelete
    {
        public static readonly Guid Id =
            Guid.Parse("22222222-2222-2222-2222-222222222222");

        public const string Email = "delete@example.com";
        public const string FirstName = "deleteUser";
        public const string Password = "DeleteUser123!";
    }
    
    public static class UserForBooking
    {
        public static readonly Guid Id =
            Guid.Parse("88888888-8888-8888-8888-888888888888");

        public const string Email = "booking@example.com";
        public const string FirstName = "bookingUser";
        public const string Password = "BookingUser123!";
    }
}