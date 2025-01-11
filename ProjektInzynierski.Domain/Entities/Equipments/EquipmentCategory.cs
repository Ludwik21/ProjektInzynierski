using System.ComponentModel;

namespace ProjektInzynierski.Domain.Entities.Equipments
{
    public enum EquipmentCategory
    {
        [Description("Lens")]
        Lens = 1,

        [Description("Camera")]
        Camera = 2,

        [Description("Tripod")]
        Tripod = 3,

        [Description("Filters")]
        Filters = 4,

        [Description("Light")]
        Light = 5
    }
}
