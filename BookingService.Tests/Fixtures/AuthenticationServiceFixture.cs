using BookingService.BLL.Options;
using BookingService.BLL.Services;
using BookingService.DAL.Interfaces;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace BookingService.Tests.Fixtures;

public class AuthenticationServiceFixture
{
    public (
        AuthenticationService Service,
        IUnitOfWork UnitOfWork,
        IUserRepository UserRepository
        ) Create()
    {
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var userRepository = Substitute.For<IUserRepository>();

        unitOfWork.UserRepository.Returns(userRepository);

        var jwtOptions = Options.Create(new JwtOptions
        {
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            Key = new string('A', 64),
            ExpirationMinutes = 60
        });

        var service = new AuthenticationService(
            jwtOptions,
            unitOfWork
        );

        return (service, unitOfWork, userRepository);
    }
}