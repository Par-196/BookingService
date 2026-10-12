using BookingService.BLL.DTO;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/auth/")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] AuthModel authModel) //TODO: FromQuery, FromRoute, FromBody
    {
        var result = await _authenticationService.RegisterUserAsync(authModel.Email, authModel.Password);

        if (!result)
        {
            return BadRequest("");
        }

        return Ok("");
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] AuthModel authModel)
    {
        var token = await _authenticationService.LoginUserAsync(authModel.Email, authModel.Password);

        if (token is null)
        {
            return BadRequest();
        }

        return Ok(token);
    }
}