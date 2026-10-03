using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/user/")]
public class UserConroller : ControllerBase
{
    private IUserService _userService;
    
    public UserConroller(IUserService userService)
    {
        _userService = userService;
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

    [HttpGet("email")]
    public async Task<IActionResult> GetUsersByEmail(string email)
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
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUserAsync(Guid id)
    {
        var user = await _userService.DeleteUserByIdAsync(id);

        if (!user)
        {
            return BadRequest();
        }

        return Ok(user);
    }
}