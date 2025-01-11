using ProjektInzynierski.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektInzynierski.Domain.Entities.CartItem
{
    public class CartItem
    {
        public  Guid EquipmentId { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public Currency PricePerDayCurrency { get; set; }
        public int Quantity { get; set; }
    }

}
