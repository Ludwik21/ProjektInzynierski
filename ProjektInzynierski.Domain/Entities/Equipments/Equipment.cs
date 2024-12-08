using ProjektInzynierski.Domain.Common;
using System.Xml.Linq;

namespace ProjektInzynierski.Domain.Entities.Equipments
{
    public class Equipment
    {
        public Equipment(Guid id,
            EquipmentCategory category,
            string name,
            string brand,
            string description,
            decimal pricePerDay,
            Currency PricePerDayCurrency,
            bool isAvailable)
        {
            Id = id;
            Category = category;
            Name = name;
            Brand = brand;
            Description = description;
            PricePerDay = pricePerDay;
            PricePerDayCurrency = PricePerDayCurrency;
            IsAvailable = isAvailable;
        }

        public Equipment(EquipmentCategory category,
            string name,
            string brand,
            string description,
            decimal pricePerDay,
            Currency PricePerDayCurrency)
        {
            if (pricePerDay < 0)
                throw new ArgumentException("Price cannot be negative");

            Id = Guid.NewGuid();
            Category = category;
            Name = name;
            Brand = brand;
            Description = description;
            PricePerDay = pricePerDay;
            PricePerDayCurrency = PricePerDayCurrency;
            IsAvailable = true;
        }

        public Guid Id { get; }
        public EquipmentCategory Category { get; }
        public string Name { get; }
        public string Brand { get; }
        public string Description { get; }
        public decimal PricePerDay { get; }
        public Currency PricePerDayCurrency { get; }
        public bool IsAvailable { get; private set; }

        public void Reserve()
        {
            IsAvailable = false;
        }
    }

}
