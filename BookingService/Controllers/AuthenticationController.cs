using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/auth/")]
public class AuthenticationController : ControllerBase
{
    public IAuthenticationService? _authenticationService { get; set; }

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserDTO userDTO) //TODO: FromQuery, FromRoute, FromBody
    {
        var result = await _authenticationService.RegisterUserAsync(userDTO);

        if (!result)
        {
            return BadRequest("");
        }

        return Ok("");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] UserDTO userDTO)
    {
        var token = await _authenticationService.LoginUserAsync(userDTO);

        if (token is null)
        {
            return BadRequest();
        }

        return Ok(token);
    }
}