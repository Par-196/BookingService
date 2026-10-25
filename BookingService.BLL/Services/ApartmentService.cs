using BookingService.BLL.Extensions.Mapping;
using BookingService.BLL.Interfaces;
using BookingService.BLL.Models;
using BookingService.DAL.Interfaces;
using BookingService.DAL.UnitOfWork;

namespace BookingService.BLL.Services;

public class ApartmentService : IApartmentService
{
    private IUnitOfWork _unitOfWork;
    public ApartmentService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApartmentDTO?> GetApartmentByIdAsync(Guid id)
    {
        var apartmentModel = await _unitOfWork.ApartmentRepository.GetApartmentByIdAsync(id);

        if(apartmentModel is null)
        {
            return null;
        }
        
        return apartmentModel.ToDto();;
    }

    public async Task<List<ApartmentDTO>> GetAllApartmentsAsync()
    {
        var listApartments = await _unitOfWork.ApartmentRepository.GetAllApartmentsAsync();
        
        return listApartments.Select(x => x.ToDto()).ToList();
    }

    public async Task<bool> CreateApartmentAsync(ApartmentDTO apartmentDto)
    {
        await _unitOfWork.ApartmentRepository.CreateApartmentAsync(apartmentDto.ToEntity());
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateApartmentAsync(ApartmentDTO apartmentDto)
    {
        var isUpdated  = await _unitOfWork.ApartmentRepository.UpdateApartmentAsync(apartmentDto.ToEntity());
        
        if (!isUpdated)
            return false;
        
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteApartmentByIdAsync(Guid id)
    {
        var isDelete = await _unitOfWork.ApartmentRepository.DeleteApartmentByIdAsync(id);

        if (!isDelete)
            return false;

        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}