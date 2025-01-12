using Microsoft.EntityFrameworkCore;
using ProjektInzynierski.Application.Services;
using ProjektInzynierski.Infrastructure.Models;
using ProjektInzynierski.Infrastructure.Repositories;

public class EquipmentCompatibilityService : IEquipmentCompatibilityService
{
    private readonly IEquipmentRepository _repository;

    public EquipmentCompatibilityService(IEquipmentRepository repository)
    {
        _repository = repository;
    }

    public async Task AddCompatibility(Guid equipmentId, Guid compatibleEquipmentId)
    {
        var compatibility = new EquipmentCompatibility
        {
            Id = Guid.NewGuid(),
            EquipmentId = equipmentId,
            CompatibleEquipmentId = compatibleEquipmentId
        };

        await _repository.AddCompatibility(compatibility);
    }

    public async Task RemoveCompatibility(Guid compatibilityId)
    {
        await _repository.RemoveCompatibility(compatibilityId);
    }

    public async Task<List<EquipmentCompatibility>> GetCompatibilities(Guid equipmentId)
    {
        return await _repository.GetCompatibilities(equipmentId);
    }



    public async Task ClearCompatibilities(Guid equipmentId)
    {
        var compatibilities = await _repository.GetCompatibilities(equipmentId);
        foreach (var compatibility in compatibilities)
        {
            await _repository.RemoveCompatibility(compatibility.Id);
        }
    }

}
