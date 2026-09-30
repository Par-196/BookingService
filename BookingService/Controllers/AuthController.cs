using BookingService.BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthorizationService = BookingService.BLL.Interfaces.IAuthorizationService;

namespace BookingService.Controllers;

[ApiController]
[Route("authorization")]
public class AuthController
{
    public IAuthorizationService IAuthorizationService { get; set; }

    public AuthController()
    {
        
    }

    // [HttpPost]
    // public Task<UserDTO?> CreateUser([FromBody] UserDTO user)
    // {
    //     
    // }
}