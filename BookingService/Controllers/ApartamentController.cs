using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.Controllers;

[ApiController]
[Route("/api/apartments/")]

public class ApartamentController : ControllerBase
{
    private IApartmentService _apartmentService;

    public ApartamentController(IApartmentService apartmentService)
    {
        _apartmentService = apartmentService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetApartmentByIdAsync(Guid id)
    {
        var apartment = await _apartmentService.GetApartmentByIdAsync(id);

        if (apartment is null)
        {
            return BadRequest();
        }

        return Ok(apartment);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApartmentsAsync()
    {
        var apartment = await _apartmentService.GetAllApartmentsAsync();

        return Ok(apartment);
    }

    [HttpPost]
    public async Task<IActionResult> CreateApartmentAsync(ApartmentDTO apartmentDto)
    {
        var result = await _apartmentService.CreateApartmentAsync(apartmentDto);
        
        if (!result)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateApartmentAsync(ApartmentDTO apartmentDto)
    {
        var result = await _apartmentService.UpdateApartmentAsync(apartmentDto);
        
        if (!result)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteApartmentByIdAsync(Guid id)
    {
        var result = await _apartmentService.DeleteApartmentByIdAsync(id);
        
        if (!result)
        {
            return BadRequest();
        }
        
        return Ok(result);
    }
}