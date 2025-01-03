﻿using ProjektInzynierski.Infrastructure.Models;

namespace ProjektInzynierski.Application.Services
{
    public interface IEquipmentCompatibilityService
    {
        Task AddCompatibility(Guid equipmentId, Guid compatibleEquipmentId);
        Task RemoveCompatibility(Guid compatibilityId);
        Task ClearCompatibilities(Guid equipmentId);
        Task<List<EquipmentCompatibility>> GetCompatibilities(Guid equipmentId);
    }
}
