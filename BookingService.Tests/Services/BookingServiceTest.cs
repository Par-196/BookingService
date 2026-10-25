using BookingService.BLL.Models;
using BookingService.DAL.Entity;
using BookingService.Tests.Fixtures;
using NSubstitute;

namespace BookingService.Tests.Services;

public class BookingServiceTest 
    : IClassFixture<BookingServiceFixture>
{

    private readonly BookingServiceFixture _fixture;

    private static readonly Booking _booking = new Booking
    {
        Id = Guid.NewGuid(),
        StartDate = DateTime.UtcNow,
        EndDate = DateTime.UtcNow,
        ApartmentId = Guid.NewGuid(),
        UserId = Guid.NewGuid(),
    };
    
    private static readonly BookingDTO _bookingDto = new BookingDTO
    {
        Id = _booking.Id,
        StartDate = _booking.StartDate,
        EndDate = _booking.EndDate,
        ApartmentId = _booking.ApartmentId,
        UserId = _booking.UserId,
    };
    
    private readonly List<Booking> _listBookings = new List<Booking>();
    
    public BookingServiceTest(BookingServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetBookingByIdAsync_RunSuccessfully()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .GetBookingByIdAsync(_booking.Id)
            .Returns(_booking);

        var result = await service.GetBookingByIdAsync(_booking.Id);

        Assert.NotNull(result);

        await bookingRepository
            .Received(1)
            .GetBookingByIdAsync(_booking.Id);
    }
    
    [Fact]
    public async Task GetBookingByIdAsync_ReturnNull()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .GetBookingByIdAsync(_booking.Id)!
            .Returns(Task.FromResult(_booking));

        var result = await service
            .GetBookingByIdAsync(Guid.NewGuid());

        Assert.Null(result);

        await bookingRepository
            .DidNotReceive()
            .GetBookingByIdAsync(_booking.Id);
    }
    
    
    [Fact]
    public async Task GetAllBookingsAsync_RunSuccessfully()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        _listBookings.Add(_booking);
        
        bookingRepository
            .GetAllBookingsAsync()
            .Returns(Task.FromResult(_listBookings));

        var result = await service.GetAllBookingsAsync();

        Assert.Single(result!);

        await bookingRepository
            .Received(1)
            .GetAllBookingsAsync();
    }
    
    [Fact]
    public async Task GetAllBookingsAsync_ReturnNull()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .GetAllBookingsAsync()
            .Returns(Task.FromResult(new List<Booking>()));

        var result = await service.GetAllBookingsAsync();

        Assert.Empty(result);

        await bookingRepository
            .Received(1)
            .GetAllBookingsAsync();
    }
    
    [Fact]
    public async Task GetBookingsByUserIdAsync_RunSuccessfully()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        _listBookings.Add(_booking);
        
        bookingRepository
            .GetBookingsByUserIdAsync(_booking.UserId)!
            .Returns(Task.FromResult(_listBookings));

        var result = await service.GetBookingsByUserIdAsync(_booking.UserId);

        Assert.Single(result!);

        await bookingRepository
            .Received(1)
            .GetBookingsByUserIdAsync(_booking.UserId);
    }
    
    [Fact]
    public async Task GetBookingsByUserIdAsync_ReturnNull()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        var userId = Guid.NewGuid();
        
        bookingRepository
            .GetBookingsByUserIdAsync(userId)!
            .Returns(Task.FromResult(new List<Booking>()));

        var result = await service.GetBookingsByUserIdAsync(userId);

        Assert.Empty(result);

        await bookingRepository
            .Received(1)
            .GetBookingsByUserIdAsync(userId);
    }
    
    [Fact]
    public async Task GetBookingsByApartmentIdAsync_RunSuccessfully()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        _listBookings.Add(_booking);
        
        bookingRepository
            .GetBookingsByApartmentIdAsync(_booking.ApartmentId)!
            .Returns(Task.FromResult(_listBookings));

        var result = await service.GetBookingsByApartmentIdAsync(_booking.ApartmentId);

        Assert.Single(result!);

        await bookingRepository
            .Received(1)
            .GetBookingsByApartmentIdAsync(_booking.ApartmentId);
    }
    
    [Fact]
    public async Task GetBookingsByApartmentIdAsync_ReturnNull()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        var apartmentId = Guid.NewGuid();
        
        bookingRepository
            .GetBookingsByApartmentIdAsync(apartmentId)!
            .Returns(Task.FromResult(new List<Booking>()));

        var result = await service.GetBookingsByApartmentIdAsync(apartmentId);

        Assert.Empty(result);

        await bookingRepository
            .Received(1)
            .GetBookingsByApartmentIdAsync(apartmentId);
    }
    
    [Fact]
    public async Task CreateBookingAsync_WasCreated()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .CreateBookingAsync(Arg.Any<Booking>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        var result = await service.CreateBookingAsync(_bookingDto);

        Assert.True(result);

        await bookingRepository
            .Received(1)
            .CreateBookingAsync(Arg.Any<Booking>());

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    
    [Fact]
    public async Task CreateBookingAsync_WasNotCreated()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .CreateBookingAsync(_booking)!
            .Returns(Task.FromResult(false));

        unitOfWork
            .SaveChangesAsync()
            .Returns(0);
        
        var result = await service.CreateBookingAsync(_bookingDto);

        Assert.False(result);

        await bookingRepository
            .DidNotReceive()
            .CreateBookingAsync(_booking);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }
    
    
    [Fact]
    public async Task UpdateBookingAsync_WasUpdated()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .UpdateBookingAsync(Arg.Any<Booking>())
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        var result = await service.UpdateBookingAsync(_bookingDto);

        Assert.True(result);

        await bookingRepository
            .Received(1)
            .UpdateBookingAsync(Arg.Any<Booking>());

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    
    [Fact]
    public async Task UpdateBookingAsync_WasNotUpdated()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .UpdateBookingAsync(_booking)!
            .Returns(Task.FromResult(false));

        unitOfWork
            .SaveChangesAsync()
            .Returns(0);
        
        var result = await service.UpdateBookingAsync(_bookingDto);

        Assert.False(result);

        await bookingRepository
            .DidNotReceive()
            .UpdateBookingAsync(_booking);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }
    
    
    [Fact]
    public async Task DeleteBookingByIdAsync_WasDeleted()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        bookingRepository
            .DeleteBookingByIdAsync(_booking.Id)
            .Returns(true);

        unitOfWork
            .SaveChangesAsync()
            .Returns(1);

        var result = await service.DeleteBookingByIdAsync(_booking.Id);

        Assert.True(result);

        await bookingRepository
            .Received(1)
            .DeleteBookingByIdAsync(_booking.Id);

        await unitOfWork
            .Received(1)
            .SaveChangesAsync();
    }
    
    [Fact]
    public async Task DeleteBookingByIdAsync_WasNotDeleted()
    {
        var(service, unitOfWork, bookingRepository) = 
            _fixture.Create();

        var bookingId = Guid.NewGuid();
        
        bookingRepository
            .DeleteBookingByIdAsync(bookingId)!
            .Returns(Task.FromResult(false));

        unitOfWork
            .SaveChangesAsync()
            .Returns(0);
        
        var result = await service.DeleteBookingByIdAsync(bookingId);

        Assert.False(result);

        await bookingRepository
            .Received(1)
            .DeleteBookingByIdAsync(bookingId);

        await unitOfWork
            .DidNotReceive()
            .SaveChangesAsync();
    }
}