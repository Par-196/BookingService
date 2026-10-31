using BookingService.BLL.Services;
using BookingService.DAL.Interfaces;
using BookingService.DAL.Repository;
using NSubstitute;

namespace BookingService.Tests.Fixtures;

public class UserServiceFixture
{
    public (
        UserService Service,
        IUnitOfWork UnitOfWork,
        IUserRepository UserRepository
        ) Create()
    {
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var userRepository = Substitute.For<IUserRepository>();

        unitOfWork.UserRepository.Returns(userRepository);

        var service = new UserService(unitOfWork);

        return (service, unitOfWork, userRepository);
    }
}