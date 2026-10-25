using BookingService.BLL.Services;
using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using NSubstitute;

namespace BookingService.Tests.Fixtures;

public class ApartmentServiceFixture
{
    public (
        ApartmentService Service,
        IUnitOfWork UnitOfWork,
        IApartmentRepository ApartmentRepository
        ) Create()
    {
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var apartmentRepository = Substitute.For<IApartmentRepository>();

        unitOfWork.ApartmentRepository.Returns(apartmentRepository);
        
        var service = new ApartmentService(unitOfWork);

        return (service, unitOfWork, apartmentRepository);
    }
}