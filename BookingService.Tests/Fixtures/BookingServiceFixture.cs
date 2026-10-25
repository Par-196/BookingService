using BookingService.BLL.Services;
using BookingService.DAL.Interfaces;
using NSubstitute;

namespace BookingService.Tests.Fixtures;


public class BookingServiceFixture
{
    public (
        BLL.Services.BookingService Service,
        IUnitOfWork UnitOfWork,
        IBookingRepository BookingRepository
        ) Create()
    {
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var bookingRepository = Substitute.For<IBookingRepository>();

        unitOfWork.BookingRepository.Returns(bookingRepository);

        var bookingService = new BLL.Services.BookingService
        (
            unitOfWork
        );

        return (bookingService, unitOfWork, bookingRepository);
    }
}