using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/authorization/")]
public class AuthController : ControllerBase
{
    public IAuthorizationService _authorizationService { get; set; }

    public AuthController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserDTO userDTO)
    {
        var result = await _authorizationService.RegisterUserAsync(userDTO);

        if (!result)
        {
            return BadRequest(false);
        }

        return Ok(true);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] UserDTO userDTO)
    {
        var token = await _authorizationService.LoginUserAsync(userDTO);

        if (token is null)
        {
            return BadRequest();
        }

        return Ok(token);
    }
}