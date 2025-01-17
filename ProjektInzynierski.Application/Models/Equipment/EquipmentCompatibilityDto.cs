﻿namespace ProjektInzynierski.Application.Models.Equipment;
public class EquipmentCompatibilityDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid CompatibleEquipmentId { get; set; }
    public string CompatibleEquipmentName { get; set; } = "Uknown";// Domyślna wartość
    public EquipmentDto CompatibleEquipment { get; set; } // Pełny obiekt kompatybilnego sprzętu
}
