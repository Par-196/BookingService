using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Context;
using BookingService.DAL.Entity;
using BookingService.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookingService.BLL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly BookingContext _context;
    private readonly IConfiguration _configuration; //TODO: use IOptions<> pattern

    public AuthenticationService(BookingContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<bool> RegisterUserAsync(UserDTO userDTO)
    {
        //TODO: validate if user with that email already exists
        
        var userModel = new User
        {
            Id = Guid.NewGuid(),
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Age = userDTO.Age,
            Email = userDTO.Email,
            Password = userDTO.Password, // TODO: hash passwords
            Role = Enum.Parse<UserRoles>(userDTO.Role.ToString()),
        };

        await _context.Users.AddAsync(userModel);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> LoginUserAsync(UserDTO userDTO)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDTO.Email && x.Password == userDTO.Password);
        
        if (user == null || user.Password != userDTO.Password)
        {
            return null;
        }
        
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
            (_configuration["Jwt:Key"]!);

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