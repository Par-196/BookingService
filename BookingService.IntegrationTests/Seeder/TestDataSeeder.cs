using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.IntegrationTests.Seeder.TestData;

namespace BookingService.IntegrationTests.Seeder;


public class TestDataSeeder
{
    private readonly BookingContext _context;

    public TestDataSeeder(BookingContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await SeedUsersAsync();
        await SeedApartmentsAsync();
        await SeedBookingsAsync();
    }

    public async Task SeedUsersAsync()
    {
        var users = new List<User>
        {
            new()
            {
                Id = TestUsers.DefaultUser.Id,
                FirstName = TestUsers.DefaultUser.FirstName,
                Email = TestUsers.DefaultUser.Email,
                Password = TestUsers.DefaultUser.Password
            },
            new()
            {
                Id = TestUsers.UserToDelete.Id,
                FirstName = TestUsers.UserToDelete.FirstName,
                Email = TestUsers.UserToDelete.Email,
                Password = TestUsers.UserToDelete.Password
            },
            new()
            {
                Id = TestUsers.UserForBooking.Id,
                FirstName = TestUsers.UserForBooking.FirstName,
                Email = TestUsers.UserForBooking.Email,
                Password = TestUsers.UserForBooking.Password
            }
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
    public async Task SeedApartmentsAsync()
    {
        var apartments = new List<Apartment>
        {
            new()
            {
                Id = TestApartments.DefaultApartment.Id,
                ApartmentType = TestApartments.DefaultApartment.ApartmentType,
                Location = TestApartments.DefaultApartment.Location,
            },
            new()
            {
                Id = TestApartments.ApartmentToDelete.Id,
                ApartmentType = TestApartments.ApartmentToDelete.ApartmentType,
                Location = TestApartments.ApartmentToDelete.Location,
            },
            new()
            {
                Id = TestApartments.ApartmentForBooking.Id,
                ApartmentType = TestApartments.ApartmentForBooking.ApartmentType,
                Location = TestApartments.ApartmentForBooking.Location,
            }
        };

        await _context.Apartments.AddRangeAsync(apartments);
        await _context.SaveChangesAsync();
    }
    public async Task SeedBookingsAsync()
    {
        var bookings = new List<Booking>
        {
            new()
            {
                Id = TestBookings.DefaultBooking.Id,
                StartDate = TestBookings.DefaultBooking.StartDate,
                EndDate = TestBookings.DefaultBooking.EndDate,
                ApartmentId = TestBookings.DefaultBooking.ApartmentId,
                UserId = TestBookings.DefaultBooking.UserId
            },
            new()
            {
                Id = TestBookings.BookingToDelete.Id,
                StartDate = TestBookings.BookingToDelete.StartDate,
                EndDate = TestBookings.BookingToDelete.EndDate,
                ApartmentId = TestBookings.BookingToDelete.ApartmentId,
                UserId = TestBookings.BookingToDelete.UserId
            }
        };

        await _context.Bookings.AddRangeAsync(bookings);
        await _context.SaveChangesAsync();
    }
}