using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.BLL.Options;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookingService.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly BookingContext _context;
    private readonly IOptions<JwtOptions> _jwtOptions;
    
    public AuthenticationService(BookingContext context, IOptions<JwtOptions> options)
    {
        _context = context;
        _jwtOptions = options;
    }

    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        //TODO: validate if user with that email already exists
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null)
        {
            throw new UserAlreadyExistsException("User with this email already exists.");
        }

        var entity = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = password
        };
        
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> LoginUserAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        
        if (user == null || user.Password != password)
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