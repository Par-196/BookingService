using BookingService.BLL.DTO;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.BLL.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/users/")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class UserConroller : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserContext _context;
    public UserConroller(IUserService userService, UserContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user is null)
        {
            return BadRequest();
        }

        return Ok(user);
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);

        if (user is null)
        {
            return BadRequest();
        }

        return Ok(user);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var user = await _userService.GetAllUsersAsync();

        return Ok(user);
    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UserDTO userDto)
    {
        var user = await _userService.UpdateUserAsync(userDto);

        if (!user)
        {
            return BadRequest();
        }

        return Ok(user);
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync()
    {
        var userId = _context.GetId();
        var user = await _userService.DeleteUserByIdAsync(userId);

        if (!user)
        {
            return BadRequest();
        }

        return Ok(user);
    }
}