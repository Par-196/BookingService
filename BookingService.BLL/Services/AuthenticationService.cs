using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Options;
using BookingService.DAL.Entity;
using BookingService.DAL.Interfaces;
using BookingService.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingService.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOptions<JwtOptions> _jwtOptions;
    
    public AuthenticationService(IOptions<JwtOptions> options, IUnitOfWork unitOfWork)
    {
        _jwtOptions = options;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
        if (user != null)
        {
            throw new UserAlreadyExistsException("User with this email already exists.");
        }

        var entity = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = password
        };

        await _unitOfWork.UserRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<string?> LoginUserAsync(string email, string password)
    {
        var user = await _unitOfWork.UserRepository.GetUser(u => u.Email == email && u.Password == password);
        
        if (user == null)
        {
            return null;
        }

        var issuer = _jwtOptions.Value.Issuer;
        var audience = _jwtOptions.Value.Audience;
        var key = Encoding.ASCII.GetBytes
            (_jwtOptions.Value.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
    
}